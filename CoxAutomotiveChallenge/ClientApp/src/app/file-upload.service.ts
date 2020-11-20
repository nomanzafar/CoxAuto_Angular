import { ImportResult, ImportSummary } from './../models/importResult';
import { Injectable, Inject  } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class FileUploadService {

  constructor(private http: HttpClient,
     @Inject('BASE_URL') private baseUrl: string) { }

  upload = (file) => {
    const fileToUpload = <File>file;
    const formData = new FormData();
    formData.append('file', fileToUpload);
    return this.http.post<ImportSummary>(this.baseUrl + 'home/ImportFileData', formData);
    //  .subscribe((response: any) => {
    //   console.log(response);
    //   return response;
    // }, error => console.log(error));
  }
}

