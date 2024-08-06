export interface EnquiriesDto {

  enquiryID: number;
  enquiryNumber: string;
  siteCode: string;
  serviceCode: string;
  unitCode: string;
  serviceQuantity: number;
  pricePerUnit: number;
  estimatedPrice: number;
  surveyorCode: string;
  statusEnquiry: string;
  createdOn: string;
  modifiedOn: string;
  sequence: number;
}
export class Student {
  constructor(public fname?: string, public lname?: string) {
  }
}
