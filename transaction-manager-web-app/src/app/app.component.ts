import { Component, OnInit, ViewChild } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatPaginatorIntl } from '@angular/material/paginator';
import { Utils } from './core/utils';
import { TransactionRecord } from './models/transactionRecord';
import { HttpService } from './services/http.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatDialog } from '@angular/material/dialog';
import { RemoveTransactionDialogComponent } from './remove-transaction-dialog/remove-transaction-dialog.component';
import { UpdateTransactionStatusDialogComponent } from './update-transaction-status-dialog/update-transaction-status-dialog.component';
import { ImportDialogComponent } from './import-dialog/import-dialog.component';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  providers: [HttpService, { provide: MatPaginatorIntl }]
})

export class AppComponent implements OnInit {

  displayedColumns: string[] = ['id', 'status', 'type', 'clientName', 'amount', 'action'];
  dataSource = new MatTableDataSource<TransactionRecord>();

  statusList = ['Pending', 'Completed', 'Cancelled'];
  typeList = ['Refill', 'Withdrawal'];

  selectedStatus: string | null = null;
  selectedType: string | null = null;

  form = new FormGroup({
    status: new FormControl(),
    type: new FormControl()
  });

  page = 1;
  count = 0;
  tableSize = 10;

  @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;

  constructor(
    private httpService: HttpService,
    private snackBar: MatSnackBar,
    public dialog: MatDialog) {
  }

  ngOnInit(): void {
    this.fetchTransactions();
  }

  ngAfterViewInit(): void {
    this.dataSource.paginator = this.paginator;
  }

  onImportClick(): void {
    const dialogRef = this.dialog.open(ImportDialogComponent, {
      width: '280px'
    });

    dialogRef.afterClosed().subscribe(result => {
      this.fetchTransactions();
    });
  }

  onSelectStatus(event: any): void {
    if (event.value === undefined) {
      this.selectedStatus = null;
    } else {
      this.selectedStatus = event.value;
    }

    this.fetchTransactions(this.selectedStatus, this.selectedType, this.page, this.tableSize);
  }

  onSelectType(event: any): void {
    if (event.value === undefined) {
      this.selectedType = null;
    } else {
      this.selectedType = event.value;
    }

    this.fetchTransactions(this.selectedStatus, this.selectedType, this.page, this.tableSize);
  }

  onExportClick(): void {
    this.httpService.export(this.selectedStatus, this.selectedType).subscribe(data => {
      this.downloadFile(data);

    }, error => {
      this.showError(error);
    });
  }

  showError(error: any): void {
    let errorMessage = '';

    if (error.error.errors == null || error.error.errors === undefined) {
      errorMessage = error.message;
    } else {
      errorMessage = error.error.errors.join(', ');
    }

    this.snackBar.open(`An error occurred while trying to load data! Details: ${errorMessage}`, undefined, {
      duration: 5000,
    });
  }

  fetchTransactions(status: string | null = null, type: string | null = null, page: number = 1, pageSize: number = 10): void {
    this.httpService.loadPagesOfNumber(this.tableSize).subscribe(data => {
      if (data.status === true) {
        if (data.data === 0) {
          this.count = 0;
        } else {
          this.count = data.data * this.tableSize;
          this.loadTransactions(status, type, 1, this.count);
        }
      }
    }, error => {
      this.showError(error);
    });
  }

  loadTransactions(status: string | null = null, type: string | null = null, page: number = 1, pageSize: number = 10): void {
    this.httpService.loadTransactionPart(status, type, 1, this.count).subscribe(data => {
      if (data.status === true) {
        this.dataSource.data = data.data;
        this.count = data.data.length;
      }
    },
      error => {
        this.showError(error);
      });
  }

  onDeleteClick(transactionId: number): void {
    const dialogRef = this.dialog.open(RemoveTransactionDialogComponent, {
      width: '370px',
      data: { transactionId: transactionId }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result === true) {
        this.fetchTransactions();
      }
    });
  }

  onEditStatusClick(transactionId: number, status: string): void {
    const dialogRef = this.dialog.open(UpdateTransactionStatusDialogComponent, {
      width: '400px',
      data: {
        transactionId: transactionId,
        currentStatus: status,
        statusList: this.statusList
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result === true) {
        this.fetchTransactions();
      }
    });
  }

  downloadFile(data: any): void {
    const blob = new Blob([data], { type: data.type });

    // IE doesn't allow using a blob object directly as link href
    // instead it is necessary to use msSaveOrOpenBlob
    if (window.navigator && window.navigator.msSaveOrOpenBlob) {
      window.navigator.msSaveOrOpenBlob(blob);
      return;
    }

    // For other browsers:
    // Create a link pointing to the ObjectURL containing the blob.
    const urlData = window.URL.createObjectURL(blob);

    let link = document.createElement('a');
    link.href = urlData;

    link.download = `Export_${new Date().toLocaleDateString()}.xlsx`;

    // this is necessary as link.click() does not work on the latest firefox
    link.dispatchEvent(new MouseEvent('click', { bubbles: true, cancelable: true, view: window }));

    setTimeout(() => {
      // For Firefox it is necessary to delay revoking the ObjectURL
      window.URL.revokeObjectURL(urlData);
      link.remove();
    }, 100);
  }
}
