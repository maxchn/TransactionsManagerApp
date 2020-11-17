import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Utils } from '../core/utils';

@Injectable()
export class HttpService {
    constructor(private http: HttpClient) { }

    loadPagesOfNumber(pageSize: number): Observable<any> {
        const params = new HttpParams().set('pageSize', pageSize.toString());
        return this.http.get(`${Utils.API_BASE_URL}v1/Transaction/GetNumberOfPages`, { params });
    }

    loadTransactionPart(status: string | null, type: string | null, page: number, pageSize: number): Observable<any> {
        let params = new HttpParams()
            .set('page', page.toString())
            .set('pageSize', pageSize.toString());

        if (status != null) {
            params = params.set('status', status);
        }

        if (type != null) {
            params = params.set('type', type);
        }

        return this.http.get(`${Utils.API_BASE_URL}v1/Transaction/GetPart`, { params });
    }

    removeTransaction(transactionId: number): Observable<any> {
        const params = new HttpParams().set('TransactionId', transactionId.toString());
        return this.http.delete(`${Utils.API_BASE_URL}v1/Transaction/Delete`, { params });
    }

    updateTransactionStatus(transactionId: number, newStatus: string): Observable<any> {
        return this.http.put(`${Utils.API_BASE_URL}v1/Transaction/Update`, {
            TransactionId: transactionId,
            Status: newStatus
        });
    }

    import(formData: FormData): Observable<any> {
        return this.http.post(`${Utils.API_BASE_URL}v1/Transaction/Import`, formData);
    }

    export(status: string | null, type: string | null): Observable<any> {
        let parameters = new HttpParams();

        const httpHeaders = new HttpHeaders()
            .set('Accept', '*/*');

        if (status != null) {
            parameters = parameters.set('status', status);
        }

        if (type != null) {
            parameters = parameters.set('type', type);
        }

        return this.http.get(`${Utils.API_BASE_URL}v1/Transaction/Export`, {
            headers: httpHeaders,
            params: parameters,
            responseType: 'blob'
        });
    }
}
