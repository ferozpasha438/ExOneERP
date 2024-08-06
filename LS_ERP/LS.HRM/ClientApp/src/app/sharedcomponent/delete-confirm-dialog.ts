import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
@Component({
  selector: 'delete-confirmation-dialog',
  templateUrl: './delete-confirm-dialog.html',
  //styleUrls: ['./confirmation-dialog.component.css']
})
export class DeleteConfirmDialogComponent {
  modalTitle: string = '';
  constructor(
    public dialogRef: MatDialogRef<DeleteConfirmDialogComponent>){ }
    //@Inject(MAT_DIALOG_DATA) public message: string) { }
  //modalTitle: string;
  onNoClick(): void {
    this.dialogRef.close();
  }
}

