export type Contract = {
  id: number;
  name: string;
  contractStatus: ContractStatus
};

enum ContractStatus {
  LinkGenerated,
  ContractSigned,
  ContractCounterSigned,
  FinalContractSentToApplicant,
}
