import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { HttpService } from '../services/http.service';
import { Utils } from '../core/utils';
import { MatSnackBar } from '@angular/material/snack-bar';

export interface DialogData {
  transactionId: number;
}

@Component({
  selector: 'app-remove-transaction-dialog',
  templateUrl: './remove-transaction-dialog.component.html',
  styleUrls: ['./remove-transaction-dialog.component.css']
})
export class RemoveTransactionDialogComponent implements OnInit {

  transactionId = -1;

  constructor(
    public dialogRef: MatDialogRef<RemoveTransactionDialogComponent>,
    private httpService: HttpService,
    private snackBar: MatSnackBar,
    @Inject(MAT_DIALOG_DATA) public data: DialogData) {

    this.transactionId = data.transactionId;
  }

  ngOnInit(): void {
  }

  onCancelClick(): void {
    this.dialogRef.close();
  }

  onRemoveClick(): void {

    if (this.transactionId === -1) {
      return;
    }

    this.httpService.removeTransaction(this.transactionId).subscribe(data => {

      if (data.status === true) {
        this.dialogRef.close(true);
      } else {
        let errorMessage;

        if (data.errors == null || data.errors === undefined) {
          errorMessage = data.errors;
        } else {
          errorMessage = data.errors.join(', ');
        }

        this.snackBar.open(`An error occurred while trying to load data! Details: ${errorMessage}`, undefined, {
          duration: 5000,
        });
      }
    }, error => {
      let errorMessage;

      if (error.error.errors == null || error.error.errors === undefined) {
        errorMessage = error.message;
      } else {
        errorMessage = error.error.errors.join(', ');
      }

      this.snackBar.open(`An error occurred while trying to load data! Details: ${errorMessage}`, undefined, {
        duration: 5000,
      });
    });
  }
}
