#pragma checksum "C:\Users\Tobi\source\repos\InfoScreenV6\ScreenCoreApp\Pages\NoContent.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "dc1412dc6b4bc6cf10c9cb5452366cce8e24b517"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(NetCoreTestApp.Pages.Pages_NoContent), @"mvc.1.0.razor-page", @"/Pages/NoContent.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure.RazorPageAttribute(@"/Pages/NoContent.cshtml", typeof(NetCoreTestApp.Pages.Pages_NoContent), null)]
namespace NetCoreTestApp.Pages
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"dc1412dc6b4bc6cf10c9cb5452366cce8e24b517", @"/Pages/NoContent.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"23457e985b2d1e6a0496892ee58d47111edb452b", @"/Pages/_ViewImports.cshtml")]
    public class Pages_NoContent : global::Microsoft.AspNetCore.Mvc.RazorPages.Page
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#line 3 "C:\Users\Tobi\source\repos\InfoScreenV6\ScreenCoreApp\Pages\NoContent.cshtml"
  
    ViewData["Title"] = "NoContent";

#line default
#line hidden
            BeginContext(95, 22, true);
            WriteLiteral("\r\n<h2>NoContent</h2>\r\n");
            EndContext();
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<ScreenCoreApp.Pages.NoContentModel> Html { get; private set; }
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<ScreenCoreApp.Pages.NoContentModel> ViewData => (global::Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<ScreenCoreApp.Pages.NoContentModel>)PageContext?.ViewData;
        public ScreenCoreApp.Pages.NoContentModel Model => ViewData.Model;
    }
}
#pragma warning restore 1591
