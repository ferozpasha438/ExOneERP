import { __decorate } from "tslib";
import { Component } from '@angular/core';
import { Validators } from '@angular/forms';
let AddupdateNotificationComponent = class AddupdateNotificationComponent {
    constructor(fb, apiService, authService, utilService, dialogRef, notifyService) {
        this.fb = fb;
        this.apiService = apiService;
        this.authService = authService;
        this.utilService = utilService;
        this.dialogRef = dialogRef;
        this.notifyService = notifyService;
        this.id = 0;
        this.isArab = false;
        this.branchCodeList = [];
        this.gradeCodeList = [];
        this.nationalityList = [];
        this.sectionList = [];
        this.schoolPETCategoryList = [];
        this.genderList = [];
        this.mobileNumber = '';
        this.isApprovalLogin = false;
        this.isShowSave = true;
    }
    ngOnInit() {
        this.form = this.fb.group({
            'branchCode': [''],
            'gradeCode': [''],
            'nationalityCode': [''],
            'sectionCode': [''],
            'ptGroupCode': [''],
            'genderCode': [''],
            'pickUpAndDropZone': [''],
            'notificationTitle': ['', Validators.required],
            'notificationTitle_Ar': ['', Validators.required],
            'notificationMessage': ['', Validators.required],
            'notificationMessage_Ar': ['', Validators.required]
        });
        this.loadList();
        if (this.row) {
            this.id = this.row['id'];
            this.editNotification();
        }
    }
    loadList() {
        this.apiService.getall('schoolBranches/getSchoolBranchList').subscribe(res => {
            this.branchCodeList = res;
        });
        this.apiService.getPagination('acedemicClassGrade', this.utilService.getQueryString(0, 1000, '', '')).subscribe(res => {
            if (res)
                this.gradeCodeList = res['items'];
        });
        this.apiService.getPagination('schoolNational', this.utilService.getQueryString(0, 1000, '', '')).subscribe(res => {
            if (res)
                this.nationalityList = res['items'];
        });
        this.apiService.getPagination('schoolPETCategory', this.utilService.getQueryString(0, 1000, '', '')).subscribe(res => {
            if (res)
                this.schoolPETCategoryList = res['items'];
        });
        this.apiService.getPagination('schoolGender', this.utilService.getQueryString(0, 1000, '', '')).subscribe(res => {
            if (res)
                this.genderList = res['items'];
        });
    }
    loadRelatedItems() {
        const gradeCode = this.form.value['gradeCode'];
        if (gradeCode != null && gradeCode != '') {
            this.apiService.getall(`SchoolGradeSectionMapping/getAllSectionsByGradeCode/${gradeCode}`).subscribe(res => {
                if (res)
                    this.sectionList = res;
            });
        }
    }
    reset() {
        this.form.reset();
    }
    approveNotification() {
        this.form.value['id'] = this.id;
        this.form.value['notificationType'] = 3;
        this.apiService.post('WebNotification/BulkWebNotificationApproval', this.form.value)
            .subscribe(res => {
            this.utilService.OkMessage();
            this.dialogRef.close(true);
        }, error => {
            console.error(error);
            this.utilService.ShowApiErrorMessage(error);
        });
    }
    closeModel() {
        this.dialogRef.close();
    }
    editNotification() {
        this.apiService.getall(`WebNotification/GetNotificationById/${this.id}`).subscribe(res => {
            if (res) {
                if (res.isApproved) {
                    this.isShowSave = false;
                }
                else {
                    this.apiService.get('TeacherMaster/IsApprovalLoginTeacher', 3).subscribe(res => {
                        if (res) {
                            this.isApprovalLogin = res;
                        }
                    });
                }
                this.id = res.id;
                this.form.patchValue({
                    'branchCode': res.branchCode,
                    'gradeCode': res.gradeCode,
                    'nationalityCode': res.nationalityCode,
                    'sectionCode': res.sectionCode,
                    'ptGroupCode': res.ptGroupCode,
                    'genderCode': res.genderCode,
                    'pickUpAndDropZone': res.pickUpAndDropZone,
                    'notificationTitle': res.notificationTitle,
                    'notificationTitle_Ar': res.notificationTitle_Ar,
                    'notificationMessage': res.notificationMessage,
                    'notificationMessage_Ar': res.notificationMessage_Ar
                });
                this.loadRelatedItems();
            }
        });
    }
    submit() {
        if (this.form.valid) {
            this.form.value['id'] = this.id;
            this.form.value['notificationType'] = 3;
            this.apiService.post('WebNotification/CreateNotification', this.form.value)
                .subscribe(res => {
                this.utilService.OkMessage();
                this.reset();
                this.dialogRef.close(true);
            }, error => {
                console.error(error);
                this.utilService.ShowApiErrorMessage(error);
            });
        }
        else
            this.utilService.FillUpFields();
    }
};
AddupdateNotificationComponent = __decorate([
    Component({
        selector: 'app-addupdate-notification',
        templateUrl: './addupdate-notification.component.html',
        styleUrls: []
    })
], AddupdateNotificationComponent);
export { AddupdateNotificationComponent };
//# sourceMappingURL=addupdate-notification.component.js.map