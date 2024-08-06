export class PaginationModel {
    selectItemsPerPage: number[] = [10, 25, 50, 100];
    pageSize = this.selectItemsPerPage[0];
    pageIndex = 0;
    allItemsLength = 0;
}

export class ReportPaginationModel {
  selectItemsPerPage: number[] = [500, 1000, 1500, 2000];
    pageSize = this.selectItemsPerPage[0];
    pageIndex = 0;   
}
