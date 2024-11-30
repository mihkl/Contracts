export type UploadFileResponse = {
  template : Template;
  guid: string;
  infoMessage: string | null;
}

export type Template = {
  id : number;
  name : string;
  fields : TemplateField[];
  creationTime : Date;
}

export type TemplateField = {
  name : string;
  type : string;
}