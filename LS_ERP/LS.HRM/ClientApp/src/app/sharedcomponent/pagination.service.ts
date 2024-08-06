import { Injectable } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { PaginationModel, ReportPaginationModel } from '../models/pagination.model';

@Injectable({
  providedIn: 'root'
})
export class PaginationService {
  private paginationModel: PaginationModel;

  get page(): number {
    return this.paginationModel.pageIndex;
  }

  get selectItemsPerPage(): number[] {
    return this.paginationModel.selectItemsPerPage;
  }

  get pageCount(): number {
    return this.paginationModel.pageSize;
  }

  constructor() {
    this.paginationModel = new PaginationModel();
  }

  change(pageEvent: PageEvent) {
    this.paginationModel.pageIndex = pageEvent.pageIndex + 1;
    this.paginationModel.pageSize = pageEvent.pageSize;
    this.paginationModel.allItemsLength = pageEvent.length;

  }
}

@Injectable({
  providedIn: 'root'
})
export class ReportPaginationService {
  private reportPaginationModel: ReportPaginationModel;

  get page(): number {
    return this.reportPaginationModel.pageIndex;
  }

  get selectItemsPerPage(): number[] {
    return this.reportPaginationModel.selectItemsPerPage;
  }

  get pageCount(): number {
    return this.reportPaginationModel.pageSize;
  }

  constructor() {
    this.reportPaginationModel = new ReportPaginationModel();
  }

}
