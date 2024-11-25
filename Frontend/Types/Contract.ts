export type Contract = {
  id: number;
  name: string;
  signingStatus: SigningStatus;
  url: string;
  linkValidFrom: string;
  linkValidUntil: string;
  creationTime : Date;
};

export enum SigningStatus {
  SignedByNone = "SignedByNone",
  SignedByFirstParty = "SignedByFirstParty",
  SignedBySecondParty = "SignedBySecondParty",
  SignedByAll = "SignedByAll",
}
