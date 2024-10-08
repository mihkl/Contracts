export type UploadFileResponse = {
  template : Template,
  guid: string;
}

export type Template = {
  id : number;
  name : string;
  fields : TemplateField[];
}

export type TemplateField = {
  name : string;
  type : string;
}