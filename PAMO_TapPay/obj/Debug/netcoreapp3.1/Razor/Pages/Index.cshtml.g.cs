#pragma checksum "D:\WorkSpace\PAMO\SourceCode\law-firm-sales-campaign\PAMO_TapPay\Pages\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "017433aeca65f8f7133c75a3f1cf7c779430c87d"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(PAMO_TapPay.Pages.Pages_Index), @"mvc.1.0.razor-page", @"/Pages/Index.cshtml")]
namespace PAMO_TapPay.Pages
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "D:\WorkSpace\PAMO\SourceCode\law-firm-sales-campaign\PAMO_TapPay\Pages\_ViewImports.cshtml"
using PAMO_TapPay;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"017433aeca65f8f7133c75a3f1cf7c779430c87d", @"/Pages/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"b8fa74f4a5cc5c9c32d25050f5631bc14a74e31e", @"/Pages/_ViewImports.cshtml")]
    public class Pages_Index : global::Microsoft.AspNetCore.Mvc.RazorPages.Page
    {
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.HeadTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_HeadTagHelper;
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.BodyTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 3 "D:\WorkSpace\PAMO\SourceCode\law-firm-sales-campaign\PAMO_TapPay\Pages\Index.cshtml"
  
    ViewData["Title"] = "付款頁面";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<!DOCTYPE html>\r\n<html lang=\"en\">\r\n\r\n");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("head", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "017433aeca65f8f7133c75a3f1cf7c779430c87d3560", async() => {
                WriteLiteral(@"
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1"">
    <link rel=""stylesheet"" href=""https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css"" integrity=""sha384-BVYiiSIFeK1dGmJRAkycuHAHRg32OmUcww7on3RYdg4Va+PmSTsz/K68vbdEjh4u"" crossorigin=""anonymous"">
    <style>
        body {
            margin: 20px 0;
        }

        .jumbotron {
            text-align: center;
        }

        .text-left {
            text-align: left;
        }

        .container {
            max-width: 750px;
        }

        form {
            padding: 40px;
            box-shadow: 0 7px 14px rgba(50, 50, 93, 0.1), 0 3px 6px rgba(0, 0, 0, 0.08);
        }

        .tappay-field-focus {
            border-color: #66afe9;
            outline: 0;
            -webkit-box-shadow: inset 0 1px 1px rgba(0, 0, 0, .075), 0 0 8px rgba(102, 175, 233, .6);
            box-shadow: inset 0 1px 1px rgba(0, 0, 0, .075), 0 0 8px rgba(102, 175, 233, .6);");
                WriteLiteral(@"
        }

        .has-error .tappay-field-focus {
            border-color: #843534;
            -webkit-box-shadow: inset 0 1px 1px rgba(0, 0, 0, .075), 0 0 6px #ce8483;
            box-shadow: inset 0 1px 1px rgba(0, 0, 0, .075), 0 0 6px #ce8483;
        }

        .has-success .tappay-field-focus {
            border-color: #2b542c;
            -webkit-box-shadow: inset 0 1px 1px rgba(0, 0, 0, .075), 0 0 6px #67b168;
            box-shadow: inset 0 1px 1px rgba(0, 0, 0, .075), 0 0 6px #67b168;
        }
    </style>
");
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_HeadTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.HeadTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_HeadTagHelper);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n\r\n");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("body", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "017433aeca65f8f7133c75a3f1cf7c779430c87d6144", async() => {
                WriteLiteral("\r\n    <div class=\"jumbotron\">\r\n        <h2>歡迎使用<br>PAMO線上付款系統</h2>\r\n        <p class=\"lead\">PAMO使用TapPay第三方服務，提供安全的線上付款方式</p>\r\n    </div>\r\n    <div class=\"container\">\r\n        ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "017433aeca65f8f7133c75a3f1cf7c779430c87d6592", async() => {
                    WriteLiteral(@"
            <div class=""form-group row"">
                <div class=""col"">
                    <label for=""FirstName"">姓</label>
                    <input class=""form-control"" id=""FirstName"" placeholder=""姓"">
                </div>
                <div class=""col"">
                    <label for=""LastName"">名</label>
                    <input class=""form-control"" id=""LastName"" placeholder=""名"">
                </div>
            </div>
            <div class=""form-group"">
                <label for=""PhoneNumber"">手機號碼</label>
                <input class=""form-control"" id=""PhoneNumber"" placeholder=""手機號碼"">
            </div>
            <div class=""form-group"">
                <label for=""Email"">電子信箱</label>
                <input type=""email"" class=""form-control"" id=""Email"" placeholder=""電子信箱"">
            </div>
            <div class=""form-group card-number-group"">
                <label for=""card-number"" class=""control-label""><span id=""cardtype""></span>卡號</label>
                <div clas");
                    WriteLiteral(@"s=""form-control card-number""></div>
            </div>
            <div class=""form-group expiration-date-group"">
                <label for=""expiration-date"" class=""control-label"">卡片到期日</label>
                <div class=""form-control expiration-date"" id=""tappay-expiration-date""></div>
            </div>
            <div class=""form-group cvc-group"">
                <label for=""cvc"" class=""control-label"">卡片後三碼</label>
                <div class=""form-control cvc""></div>
            </div>

            <button type=""submit"" class=""btn btn-default"">Pay</button>
        ");
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper);
                __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral(@"
        <br>
    </div>

    <script src=""https://code.jquery.com/jquery-3.2.1.min.js"" integrity=""sha256-hwg4gsxgFZhOsEEamdOYGBf13FyQuiTwlAQgxVSNgt4="" crossorigin=""anonymous""></script>
    <script src=""https://js.tappaysdk.com/tpdirect/v5.8.0""></script>
    <script src=""https://unpkg.com/sweetalert/dist/sweetalert.min.js""></script>
    <script>
        TPDirect.setupSDK(122018, 'app_X9KbcygzQy1exnnqBdeblLhcw4qzeQJwobg4Oa4a6lRLGlBYED90vqzuSQiq', 'sandbox')
        TPDirect.card.setup({
            fields: {
                number: {
                    element: '.form-control.card-number',
                    placeholder: '**** **** **** ****'
                },
                expirationDate: {
                    element: document.getElementById('tappay-expiration-date'),
                    placeholder: 'MM / YY'
                },
                ccv: {
                    element: $('.form-control.cvc')[0],
                    placeholder: '後三碼'
                }
            },
  ");
                WriteLiteral(@"          styles: {
                'input': {
                    'color': 'gray'
                },
                'input.ccv': {
                    // 'font-size': '16px'
                },
                ':focus': {
                    'color': 'black'
                },
                '.valid': {
                    'color': 'green'
                },
                '.invalid': {
                    'color': 'red'
                }
            }
        })
        // listen for TapPay Field
        TPDirect.card.onUpdate(function (update) {
            /* Disable / enable submit button depend on update.canGetPrime  */
            /* ============================================================ */

            // update.canGetPrime === true
            //     --> you can call TPDirect.card.getPrime()
            // const submitButton = document.querySelector('button[type=""submit""]')
            if (update.canGetPrime) {
                // submitButton.removeAttribute('disabl");
                WriteLiteral(@"ed')
                $('button[type=""submit""]').removeAttr('disabled')
            } else {
                // submitButton.setAttribute('disabled', true)
                $('button[type=""submit""]').attr('disabled', true)
            }


            /* Change card type display when card type change */
            /* ============================================== */

            // cardTypes = ['visa', 'mastercard', ...]
            var newType = update.cardType === 'unknown' ? '' : update.cardType
            $('#cardtype').text(newType)



            /* Change form-group style when tappay field status change */
            /* ======================================================= */

            // number 欄位是錯誤的
            if (update.status.number === 2) {
                setNumberFormGroupToError('.card-number-group')
            } else if (update.status.number === 0) {
                setNumberFormGroupToSuccess('.card-number-group')
            } else {
                setNumbe");
                WriteLiteral(@"rFormGroupToNormal('.card-number-group')
            }

            if (update.status.expiry === 2) {
                setNumberFormGroupToError('.expiration-date-group')
            } else if (update.status.expiry === 0) {
                setNumberFormGroupToSuccess('.expiration-date-group')
            } else {
                setNumberFormGroupToNormal('.expiration-date-group')
            }

            if (update.status.cvc === 2) {
                setNumberFormGroupToError('.cvc-group')
            } else if (update.status.cvc === 0) {
                setNumberFormGroupToSuccess('.cvc-group')
            } else {
                setNumberFormGroupToNormal('.cvc-group')
            }
        })

        $('form').on('submit', function (event) {
            event.preventDefault()

            // fix keyboard issue in iOS device
            forceBlurIos()

            const tappayStatus = TPDirect.card.getTappayFieldsStatus()
            console.log(tappayStatus)

            //");
                WriteLiteral(@" Check TPDirect.card.getTappayFieldsStatus().canGetPrime before TPDirect.card.getPrime
            if (tappayStatus.canGetPrime === false) {
                alert('can not get prime')
                return
            }

            // Get prime
            TPDirect.card.getPrime(function (result) {
                if (result.status !== 0) {
                    alert('get prime error ' + result.msg)
                    return
                }
                //TODO 測試用，上線前改為POST
                var url = ""Index?handler=SnedData"";
                url += ""&Prime="" + result.card.prime;
                url += ""&FirstName="" + $('#FirstName').val()
                url += ""&LastName="" + $('#LastName').val()
                url += ""&PhoneNumber="" + $('#PhoneNumber').val()
                url += ""&Email="" + $('#Email').val()
                $.get(url).done(function (res) {
                    var obj = jQuery.parseJSON(res);
                    if (obj.status == 0) {
                        swa");
                WriteLiteral(@"l(""Success!"", obj.msg, ""success"", { button: ""Cancel"" });
                    } else {
                        swal(""Error!"", obj.msg, ""error"", { button: ""Cancel"" });
                    }
                });
            })
        })

        function setNumberFormGroupToError(selector) {
            $(selector).addClass('has-error')
            $(selector).removeClass('has-success')
        }

        function setNumberFormGroupToSuccess(selector) {
            $(selector).removeClass('has-error')
            $(selector).addClass('has-success')
        }

        function setNumberFormGroupToNormal(selector) {
            $(selector).removeClass('has-error')
            $(selector).removeClass('has-success')
        }

        function forceBlurIos() {
            if (!isIos()) {
                return
            }
            var input = document.createElement('input')
            input.setAttribute('type', 'text')
            // Insert to active element to ensure scroll lands ");
                WriteLiteral(@"somewhere relevant
            document.activeElement.prepend(input)
            input.focus()
            input.parentNode.removeChild(input)
        }

        function isIos() {
            return /iPad|iPhone|iPod/.test(navigator.userAgent) && !window.MSStream;
        }

    </script>
");
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.BodyTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n\r\n</html>");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<IndexModel> Html { get; private set; }
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<IndexModel> ViewData => (global::Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<IndexModel>)PageContext?.ViewData;
        public IndexModel Model => ViewData.Model;
    }
}
#pragma warning restore 1591
