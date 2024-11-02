export type Contract = {
  id: number;
  name: string;
  signingStatus: SigningStatus;
};

enum SigningStatus {
  SignedByNone,
  SignedByFirstParty,
  SignedBySecondParty,
  SignedByAll,
}
