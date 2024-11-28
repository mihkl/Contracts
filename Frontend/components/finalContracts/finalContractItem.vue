<template>
  <div :class="['p-4 border rounded-lg transition bg-white border-gray-300']">
    <div class="flex flex-row justify-between items-center">
      <span class="mb-4">{{ contract?.name }}</span>
    </div>
    <div class="flex flex-row justify-start items-center gap-3">
      <UTooltip
        :text="contractStatuses[index]"
        v-for="index in [0, 1, 2, 3]"
        :popper="{ placement: 'top' }"
      >
        <SvgoContractProgress
          :class="[
            '!w-20',
            'text-[#C9C9C9]',
            '!h-auto',
            'cursor-pointer',
            index <= signingStatuses.indexOf(contract.signingStatus)
              ? 'highlighted'
              : '',
            'contract-progress-svg',
          ]"
        />
      </UTooltip>
    </div>
  </div>
</template>

<script setup lang="ts">
import { SigningStatus, type Contract } from "../../Types/Contract.js";

const contractStatuses = [
  "Link Generated",
  "Contract Signed",
  "Contract Countersigned",
  "Final Contract Sent To Applicant",
];

const signingStatuses: SigningStatus[] = [
  SigningStatus.SignedByNone,
  SigningStatus.SignedByFirstParty,
  SigningStatus.SignedBySecondParty,
  SigningStatus.SignedByAll,
];

defineProps<{
  contract: Contract;
}>();
</script>
