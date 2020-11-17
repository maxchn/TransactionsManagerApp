import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { HttpService } from '../services/http.service';
import { Utils } from '../core/utils';
import { MatSnackBar } from '@angular/material/snack-bar';
import { FormBuilder } from '@angular/forms';
import { FormGroup } from '@angular/forms';
import { FormControl } from '@angular/forms';
import { Validators } from '@angular/forms';

export interface DialogData {
  transactionId: number;
  currentStatus: string;
  statusList: any[]
}

@Component({
  selector: 'app-update-transaction-status-dialog',
  templateUrl: './update-transaction-status-dialog.component.html',
  styleUrls: ['./update-transaction-status-dialog.component.css']
})
export class UpdateTransactionStatusDialogComponent implements OnInit {

  updateTransactionStatusForm: FormGroup;
  selectedStatus = '';

  statusList: any[];
  transactionId = -1;
  currrentStatus = '';

  constructor(
    fb: FormBuilder,
    public dialogRef: MatDialogRef<UpdateTransactionStatusDialogComponent>,
    private httpService: HttpService,
    private snackBar: MatSnackBar,
    @Inject(MAT_DIALOG_DATA) public data: DialogData) {

    this.statusList = data.statusList;
    this.transactionId = data.transactionId;
    this.currrentStatus = data.currentStatus;

    this.updateTransactionStatusForm = fb.group({
      hideRequired: false,
      floatLabel: 'auto',
    });
  }

  public hasError = (controlName: string, errorName: string) => {
    return this.updateTransactionStatusForm.controls[controlName].hasError(errorName);
  }

  ngOnInit(): void {
    this.updateTransactionStatusForm = new FormGroup({
      status: new FormControl(this.currrentStatus,
        [
          Validators.required
        ]),
    });
  }

  onSelectStatus(event: any): void {
    this.selectedStatus = event.value;
  }

  onCancelClick(): void {
    this.dialogRef.close();
  }

  onSaveClick(): void {

    if (this.transactionId === -1 || this.selectedStatus === '') {
      return;
    }

    this.httpService.updateTransactionStatus(this.transactionId, this.selectedStatus).subscribe(data => {
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
