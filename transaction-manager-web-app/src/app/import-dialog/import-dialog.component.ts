import { Component, OnInit, Inject } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { FormBuilder } from '@angular/forms';
import { FormControl } from '@angular/forms';
import { Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { HttpService } from '../services/http.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Utils } from '../core/utils';

@Component({
  selector: 'app-import-dialog',
  templateUrl: './import-dialog.component.html',
  styleUrls: ['./import-dialog.component.css']
})
export class ImportDialogComponent implements OnInit {

  importDataForm: FormGroup;

  constructor(
    fb: FormBuilder,
    public dialogRef: MatDialogRef<ImportDialogComponent>,
    private httpService: HttpService,
    private snackBar: MatSnackBar,
    @Inject(MAT_DIALOG_DATA) public data: any) {

    this.importDataForm = fb.group({
      hideRequired: false,
      floatLabel: 'auto',
    });
  }

  public hasError = (controlName: string, errorName: string) => {
    return this.importDataForm.controls[controlName].hasError(errorName);
  }

  ngOnInit(): void {
    this.importDataForm = new FormGroup({
      importFile: new FormControl('',
        [
          Validators.required
        ]),
    });
  }

  onImportFileChange(event: any): void {
    if (event.target.files.length > 0) {
      const file = event.target.files[0];
      this.importDataForm.get('importFile')?.setValue(file);
    }
  }

  onCancelClick(): void {
    this.dialogRef.close();
  }

  onSubmit(): void {
    if (this.importDataForm.get('importFile')?.value === '') {
      return;
    }

    const formData = new FormData();
    formData.append('file', this.importDataForm.get('importFile')?.value);

    this.httpService.import(formData).subscribe(data => {
      if (data.status === true) {
        this.dialogRef.close();
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
