export type Contract = {
  id: number;
  name: string;
  signingStatus: SigningStatus;
};

export enum SigningStatus {
  SignedByNone = "SignedByNone",
  SignedByFirstParty = "v",
  SignedBySecondParty = "SignedBySecondParty",
  SignedByAll = "SignedByAll",
}
