import { FileUploadService } from './../file-upload.service';
import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { ImportErrors, ImportResult, ImportSummary, Summary } from 'src/models/importResult';


@Component({
  selector: 'app-file-upload',
  templateUrl: './file-upload.component.html',
  styleUrls: ['./file-upload.component.css']
})
export class FileUploadComponent implements OnInit {

  result: ImportSummary;
  importResult: ImportResult[];
  summary: Summary[];
  errors: ImportErrors[];
  isDataDisplayed = false;
  constructor(private fileUploadService: FileUploadService) { }

  ngOnInit() {
  }

  public uploadFile = (event) => {

    const file: FileList = event.target.files[0];
    const res = this.fileUploadService.upload(file)
      .subscribe(arg => {
        this.result = arg;
        this.isDataDisplayed = true;
      });
  }

  allowUpload = () => {
    this.isDataDisplayed = false;
    this.result = null;
  }
}
