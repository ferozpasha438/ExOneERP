import { __decorate } from "tslib";
import { Component } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { debounceTime, distinctUntilChanged, map, startWith, switchMap } from 'rxjs/operators';
let AddupdateSchoolBranchComponent = class AddupdateSchoolBranchComponent {
    constructor(fb, apiService, utilService, validationService, dialog, notifyService, dialogRef) {
        this.fb = fb;
        this.apiService = apiService;
        this.utilService = utilService;
        this.validationService = validationService;
        this.dialog = dialog;
        this.notifyService = notifyService;
        this.dialogRef = dialogRef;
        this.id = 0;
        this.isBranchLoading = false;
        this.branchCodeControl = new FormControl();
        this.options = [];
        this.userList = [];
        this.branchCode = '';
        this.moderatorList = [];
        this.filteredOptions = this.branchCodeControl.valueChanges.pipe(startWith(''), debounceTime(utilService.autoDelay()), distinctUntilChanged(), switchMap((val) => {
            if (val.trim() !== '')
                this.isBranchLoading = true;
            return this.filter(val || '');
        }));
    }
    ngOnInit() {
        this.setForm();
        if (this.row) {
            this.id = parseInt(this.row['id']);
            this.branchCode = this.row['branchCode'];
            this.getModerators();
            this.setEditForm();
        }
    }
    getModerators() {
        this.apiService.getall(`TeacherMaster/GetModerators`).subscribe(res => {
            this.moderatorList = res;
        });
    }
    getTeachersByBranchCode(branchCode) {
        this.apiService.getall(`TeacherMaster/GetTeachersByBranchcode/${branchCode}`).subscribe(res => {
            this.userList = res;
        });
    }
    filter(val) {
        return this.apiService.getall(`branch/getSelectSysBranchList?search=${val.trim()}`)
            .pipe(map(response => {
            const res = response;
            this.isBranchLoading = false;
            return res;
        }));
    }
    setForm() {
        this.form = this.fb.group({
            'branchName': ['', Validators.required],
            'branchNameAr': ['', Validators.required],
            'startAcademicDate': ['', Validators.required],
            'endAcademicDate': ['', Validators.required],
            'branchPrefix': ['', Validators.required],
            'address': ['', Validators.required],
            'city': ['', Validators.required],
            'mobile': ['', Validators.required],
            'phone': ['', Validators.required],
            'email': ['', Validators.required],
            'weekOff1': ['', Validators.required],
            'weekOff2': [''],
            'geoLat': ['', Validators.required],
            'geoLong': ['', Validators.required],
            'currencyCode': [''],
            'privacyPolicyUrl': [''],
            'startWeekDay': ['', Validators.required],
            'website': [''],
            'branchNotification_Moderator': [''],
            'default_InTime': ['', Validators.required],
            'default_OutTime': ['', Validators.required],
            'schoolBranchesAuthorityList': this.fb.array([this.createAuthority()])
        });
    }
    createAuthority(res) {
        if (res) {
            return this.fb.group(res);
        }
        return this.fb.group({
            'id': 0,
            'teacherCode': [''],
            'level': [0],
            'isApproveLeave': [false],
            'isApproveDisciPlinaryAction': [false],
            'isApproveNotification': [false],
            'isApproveEvent': [false]
        });
    }
    get schoolBranchesAuthorityList() {
        return this.form.get('schoolBranchesAuthorityList');
    }
    addItem() {
        this.schoolBranchesAuthorityList.push(this.createAuthority());
    }
    editItem(res) {
        this.schoolBranchesAuthorityList.push(this.createAuthority(res));
    }
    removeItem(item) {
        this.schoolBranchesAuthorityList.removeAt(item);
    }
    selectedOption(event) {
        this.branchCode = event.option.value;
        this.getTeachersByBranchCode(event.option.value);
        this.getBranchData(event.option.value);
        this.setEditForm();
    }
    getBranchData(branchCode) {
        this.apiService.getall(`branch/getBranchDataByBranchCode?branchCode=${branchCode}`).subscribe(res => {
            if (res) {
                this.form.patchValue({
                    'branchName': res['branchName'], 'address': res['branchAddress'], 'city': res['city'], 'mobile': res['mobile'], 'phone': res['phone'],
                    'geoLat': res['geoLocLatitude'],
                    'geoLong': res['geoLocLongitude']
                });
            }
        }, error => {
            this.form.patchValue({
                'branchName': '', 'address': '', 'city': '', 'mobile': '', 'phone': '',
                'geoLat': '',
                'geoLong': ''
            });
        });
    }
    branchcodeBlur(event) {
        // this.getUsersByBranchCode(event.target.value);
        this.apiService.getall(`branch/getBranchDataByBranchCode?branchCode=${event.target.value}`).subscribe(res => {
            if (res) {
                this.form.patchValue({
                    'branchName': res['branchName'], 'address': res['branchAddress'], 'city': res['city'], 'mobile': res['mobile'], 'phone': res['phone'],
                    'geoLat': res['geoLocLatitude'],
                    'geoLong': res['geoLocLongitude']
                });
            }
        }, error => {
            this.form.patchValue({
                'branchName': '', 'address': '', 'city': '', 'mobile': '', 'phone': '',
                'geoLat': '',
                'geoLong': ''
            });
            this.branchCodeControl.setValue('');
        });
    }
    setEditForm() {
        this.apiService.getall(`branch/${this.branchCode}`).subscribe(res => {
            if (res) {
                if (res.branchCode != null) {
                    this.id = res.id;
                    this.getTeachersByBranchCode(res.branchCode);
                    //this.getBranchData(res.branchCode);
                    //this.form.patchValue(res);
                    this.form.patchValue({
                        'branchName': res.branchName,
                        'branchNameAr': res.branchNameAr,
                        'startAcademicDate': res.startAcademicDate,
                        'endAcademicDate': res.endAcademicDate,
                        'branchPrefix': res.branchPrefix,
                        'address': res.address,
                        'city': res.city,
                        'mobile': res.mobile,
                        'phone': res.phone,
                        'email': res.email,
                        'weekOff1': res.weekOff1,
                        'weekOff2': res.weekOff2,
                        'geoLat': res.geoLat,
                        'geoLong': res.geoLong,
                        'currencyCode': res.currencyCode,
                        'privacyPolicyUrl': res.privacyPolicyUrl,
                        'startWeekDay': res.startWeekDay,
                        'website': res.Website,
                        'branchNotification_Moderator': res.branchNotification_Moderator,
                        'default_InTime': res.default_InTime,
                        'default_OutTime': res.default_OutTime
                    });
                    this.branchCodeControl.setValue(res.branchCode);
                    this.schoolBranchesAuthorityList.clear();
                    let schoolBranchesAuthorityList = res.schoolBranchesAuthorityList;
                    schoolBranchesAuthorityList.forEach(item => {
                        this.editItem(item);
                    });
                }
            }
        });
    }
    submit() {
        if (this.form.valid) {
            let branchCode = this.branchCodeControl.value;
            if (branchCode && branchCode.trim() !== '') {
                this.form.value['id'] = this.id;
                this.form.value['branchCode'] = branchCode;
                this.apiService.post('Branch', this.form.value)
                    .subscribe(res => {
                    this.utilService.OkMessage();
                    this.reset();
                    this.dialogRef.close(true);
                }, error => {
                    this.utilService.ShowApiErrorMessage(error);
                });
            }
            else
                this.utilService.FillUpFields();
        }
        else
            this.utilService.FillUpFields();
    }
    reset() {
        this.form.controls['branchName'].setValue('');
        this.form.controls['branchNameAr'].setValue('');
        this.form.controls['branchPrefix'].setValue('');
        this.form.controls['address'].setValue('');
        this.form.controls['phone'].setValue('');
        this.form.controls['city'].setValue('');
        this.form.controls['mobile'].setValue('');
    }
    closeModel() {
        this.dialogRef.close();
    }
};
AddupdateSchoolBranchComponent = __decorate([
    Component({
        selector: 'app-addupdate-school-branch',
        templateUrl: './addupdate-school-branch.component.html',
        styleUrls: []
    })
], AddupdateSchoolBranchComponent);
export { AddupdateSchoolBranchComponent };
//# sourceMappingURL=addupdate-school-branch.component.js.map