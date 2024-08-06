export interface CINServerMetaDataDto {
  id: number,
  apiEndpoint: string,
  dbConnectionString: string,
  cinNumber: string
  moduleCodes: string;
  admUrl: string;
  finUrl: string;
  opmUrl: string;
  invUrl: string;
  sndUrl: string;
  posUrl: string;
  popUrl: string;
  schUrl: string;
  scpUrl: string;
  sctUrl: string;
  hrmUrl: string;
  hraUrl: string;
  hrsUrl: string;
  fltUrl: string;
  mfgUrl: string;
  crmUrl: string;
  utlUrl: string;

  //setupUri: string;
  //financeUri: string;
  //operationUri: string;
  //inventoryUri: string;
  //salesUri: string;
  //purchaseUri: string;
  //schoolUri: string;

}

export interface ApiMessageDto {
  message: string;
  type: number;
}
