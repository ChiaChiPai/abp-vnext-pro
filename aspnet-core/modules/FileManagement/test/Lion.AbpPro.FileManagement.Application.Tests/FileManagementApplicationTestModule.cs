using Volo.Abp.Modularity;

namespace Lion.AbpPro.FileManagement;

[DependsOn(
    typeof(FileManagementApplicationModule),
    typeof(FileManagementDomainTestModule)
)]
public class FileManagementApplicationTestModule : AbpModule
{
}