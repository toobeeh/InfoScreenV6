#pragma checksum "C:\Users\Tobi\source\repos\InfoScreenV6\ScreenCoreApp\Pages\ContentPages\PowerPoint.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "461874bb4e45276007274fe7213854b2832ae065"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(NetCoreTestApp.Pages.ContentPages.Pages_ContentPages_PowerPoint), @"mvc.1.0.razor-page", @"/Pages/ContentPages/PowerPoint.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure.RazorPageAttribute(@"/Pages/ContentPages/PowerPoint.cshtml", typeof(NetCoreTestApp.Pages.ContentPages.Pages_ContentPages_PowerPoint), @"{slide}")]
namespace NetCoreTestApp.Pages.ContentPages
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemMetadataAttribute("RouteTemplate", "{slide}")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"461874bb4e45276007274fe7213854b2832ae065", @"/Pages/ContentPages/PowerPoint.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"23457e985b2d1e6a0496892ee58d47111edb452b", @"/Pages/_ViewImports.cshtml")]
    public class Pages_ContentPages_PowerPoint : global::Microsoft.AspNetCore.Mvc.RazorPages.Page
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#line 3 "C:\Users\Tobi\source\repos\InfoScreenV6\ScreenCoreApp\Pages\ContentPages\PowerPoint.cshtml"
  
    ViewData["Title"] = "PowerPoint";

#line default
#line hidden
            BeginContext(120, 140, true);
            WriteLiteral("<div id=\"image_container\">\r\n\r\n</div>\r\n<script>\r\n    //image style is optimized for 1024x768 resolution on infoscreens\r\n\r\n    var slide_id = ");
            EndContext();
            BeginContext(261, 11, false);
#line 12 "C:\Users\Tobi\source\repos\InfoScreenV6\ScreenCoreApp\Pages\ContentPages\PowerPoint.cshtml"
              Write(Model.Slide);

#line default
#line hidden
            EndContext();
            BeginContext(272, 26, true);
            WriteLiteral(";\r\n    var presentation = ");
            EndContext();
            BeginContext(299, 13, false);
#line 13 "C:\Users\Tobi\source\repos\InfoScreenV6\ScreenCoreApp\Pages\ContentPages\PowerPoint.cshtml"
                  Write(Model.Webpath);

#line default
#line hidden
            EndContext();
            BeginContext(312, 352, true);
            WriteLiteral(@";
    var slide_preload = parent.document.getElementById(slide_id);

    if (slide_preload == null || !slide_preload.innerHTML.includes(presentation)) parent.location.reload(); // Reload parent page if source picture couldnt be found
    
    document.getElementById(""image_container"").innerHTML = slide_preload.innerHTML;


</script>




");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<ScreenCoreApp.Pages.ContentPages.PowerPointModel> Html { get; private set; }
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<ScreenCoreApp.Pages.ContentPages.PowerPointModel> ViewData => (global::Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<ScreenCoreApp.Pages.ContentPages.PowerPointModel>)PageContext?.ViewData;
        public ScreenCoreApp.Pages.ContentPages.PowerPointModel Model => ViewData.Model;
    }
}
#pragma warning restore 1591
