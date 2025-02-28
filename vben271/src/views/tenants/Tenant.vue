<template>
  <div>
    <BasicTable @register="registerTable" size="small">
      <template #toolbar>
        <a-button
          preIcon="ant-design:plus-circle-outlined"
          type="primary"
          @click="openCreateTenantModal"
          v-auth="'AbpTenantManagement.Tenants.Create'"
        >
          {{ t('common.createText') }}
        </a-button>
      </template>

      <template #action="{ action }">
        <TableAction
          :actions="[
            {
              icon: 'ant-design:edit-outlined',
              auth: 'AbpTenantManagement.Tenants.Update',
              label: t('common.editText'),
              onClick: handleEdit.bind(null, record),
            },
          ]"
          :dropDownActions="[
            {
              auth: 'AbpTenantManagement.Tenants.ManageConnectionStrings',
              label: t('routes.tenant.connectionString'),
              onClick: handleConnectionString.bind(null, record),
            },
            {
              auth: 'AbpTenantManagement.Tenants.Delete',
              label: t('common.delText'),
              onClick: handleDelete.bind(null, record),
            },
          ]"
        />
      </template>

      <!-- <template #action="{ record }">
        <a-button
          type="link"
          size="small"
          @click="handleEdit(record)"
          v-auth="'AbpTenantManagement.Tenants.Update'"
        >
          {{ t('common.editText') }}
        </a-button>

        <a-button
          type="link"
          size="small"
          @click="handleDelete(record)"
          v-auth="'AbpTenantManagement.Tenants.Delete'"
        >
          {{ t('common.delText') }}
        </a-button>
        <a-button
          type="link"
          size="small"
          @click="handleConnectionString(record)"
          v-auth="'AbpTenantManagement.Tenants.ManageConnectionStrings'"
        >
          {{ t('routes.tenant.connectionString') }}
        </a-button>
      </template> -->
    </BasicTable>
    <EditTenant
      @register="registerEditTenantModal"
      @reload="reload"
      :bodyStyle="{ 'padding-top': '0' }"
    />
    <CreateTenant
      @register="registerCreateTenantModal"
      @reload="reload"
      :bodyStyle="{ 'padding-top': '0' }"
    />
    <EditConnectionString
      @register="registerEditConnectionStringModal"
      @reload="reload"
      :bodyStyle="{ 'padding-top': '0' }"
    />
  </div>
</template>

<script lang="ts">
  import { defineComponent } from 'vue';
  import { useI18n } from '/@/hooks/web/useI18n';
  import { BasicModal, useModal } from '/@/components/Modal';
  import { BasicTable, useTable, TableAction } from '/@/components/Table';

  import { getTenantListAsync, tableColumns, searchFormSchema, deleteTenantAsync } from './Tenant';
  import CreateTenant from './CreateTenant.vue';

  import EditConnectionString from './EditConnectionString.vue';
  import EditTenant from './EditTenant.vue';
  import { useMessage } from '/@/hooks/web/useMessage';
  export default defineComponent({
    name: 'Tenant',
    components: {
      BasicTable,
      BasicModal,
      TableAction,
      CreateTenant,
      EditTenant,
      EditConnectionString,
    },
    setup() {
      const { t } = useI18n();

      const [registerCreateTenantModal, { openModal: openCreateTenantModal }] = useModal();
      const [registerEditTenantModal, { openModal: openEditTenantModal }] = useModal();
      const [registerEditConnectionStringModal, { openModal: openEditConnectionStringModal }] =
        useModal();
      const [registerTable, { reload }] = useTable({
        columns: tableColumns,
        formConfig: {
          labelWidth: 100,
          schemas: searchFormSchema,
        },
        api: getTenantListAsync,
        useSearchForm: true,
        showTableSetting: true,
        bordered: true,
        canResize: true,
        showIndexColumn: true,
        actionColumn: {
          title: t('common.action'),
          dataIndex: 'action',
          slots: {
            customRender: 'action',
          },
          width: 120,
          fixed: 'right',
        },
      });
      const { createConfirm } = useMessage();
      // 编辑用户
      const handleEdit = (record: Recordable) => {
        openEditTenantModal(true, { record: record });
      };

      // 删除用户
      const handleDelete = async (record: Recordable) => {
        let msg = t('common.askDelete');
        createConfirm({
          iconType: 'warning',
          title: t('common.tip'),
          content: msg,
          onOk: async () => {
            await deleteTenantAsync({ id: record.id });
            reload();
          },
        });
      };

      const handleConnectionString = async (record: Recordable) => {
        openEditConnectionStringModal(true, { record: record });
      };
      return {
        t,
        registerTable,
        reload,
        openEditTenantModal,
        openCreateTenantModal,
        registerCreateTenantModal,
        handleConnectionString,
        handleDelete,
        handleEdit,
        registerEditTenantModal,
        registerEditConnectionStringModal,
      };
    },
  });
</script>

<style lang="less" scoped></style>
