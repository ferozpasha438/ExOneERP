////export const ApiEndPoint = localStorage.getItem('apiEndpoint') ?? '';
////export const DbConnectionString = localStorage.getItem('dbConnectionString') ?? '';

export enum DBOperation {
  create,
  update,
  delete
}
export const ErrorMessage = 'Something went wrong';

//Payroll Module
export enum PayrollComponentType
{
    Earning = 1,
    Deduction = 2,
    UnStructuredEarning = 3,
    UnStructuredDeduction = 4
}

//Human Resource
export enum TypeOfLeave {
  Accrual = 1,
  ProRata = 2,
}