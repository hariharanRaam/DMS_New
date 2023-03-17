DocumentUltimate v4.6.5.0
ASP.NET Document Viewer and Converter
Copyright C 2016-2019 GleamTech
https://www.gleamtech.com/documentultimate

Version 4.6.5 Release Notes:
https://docs.gleamtech.com/documentultimate/html/version-history.htm#v4.6.5

Online Documentation:
https://docs.gleamtech.com/documentultimate/

Support Portal:
https://support.gleamtech.com/

------------------------------------------------------------------------------------------------------
To use DocumentUltimate in an ASP.NET MVC Project, do the following in Visual Studio:
------------------------------------------------------------------------------------------------------

1. Set DocumentUltimate's global configuration. For example, you may want to set the license key and 
    the cache path. Insert some of the following lines (if overriding a default value is required) into 
    the Application_Start method of your Global.asax.cs:

    ----------------------------------
    //Set this property only if you have a valid license key, otherwise do not
    //set it so DocumentUltimate runs in trial mode.
    DocumentUltimateConfiguration.Current.LicenseKey = "QQJDJLJP34...";

    //The default CacheLocation value is "~/App_Data/DocumentCache"
    //Both virtual and physical paths are allowed (or a Location instance for one of the supported 
    //file systems like Amazon S3 and Azure Blob).
    DocumentUltimateWebConfiguration.Current.CacheLocation = "~/App_Data/DocumentCache";
    ----------------------------------

    Alternatively you can specify the configuration in <appSettings> tag of your Web.config.

    ----------------------------------
    <add key="DocumentUltimate:LicenseKey" value="QQJDJLJP34..." />
    <add key="DocumentUltimateWeb:CacheLocation" value="~/App_Data/DocumentCache" />
    ----------------------------------

    As you would notice, DocumentUltimate: prefix maps to DocumentUltimateConfiguration.Current and 
    DocumentUltimateWeb: prefix maps to DocumentUltimateWebConfiguration.Current.	   

2. Open one of your View pages (eg. Index.cshtml) and at the top of
    your page add the necessary namespaces:

    ----------------------------------
    @using GleamTech.AspNet.Mvc
    @using GleamTech.DocumentUltimate
    @using GleamTech.DocumentUltimate.AspNet
    @using GleamTech.DocumentUltimate.AspNet.UI
    ----------------------------------

    Alternatively you can add the namespaces globally in Views/Web.config under 
    <system.web.webPages.razor>/<pages>/<namespaces> tag to avoid adding namespaces 
    in your pages every time:

    ----------------------------------
    <add namespace="GleamTech.AspNet.Mvc" />
    <add namespace="GleamTech.DocumentUltimate" />
    <add namespace="GleamTech.DocumentUltimate.AspNet" />
    <add namespace="GleamTech.DocumentUltimate.AspNet.UI" />
    ----------------------------------

    Now in your page insert these lines:

    ----------------------------------
    <!DOCTYPE html>
    @{
        var documentViewer = new DocumentViewer
        {
            Width = 800,
            Height = 600,
            Document = "~/Documents/Document.docx"
        };
    }
    <html>
        <head>
            <title>Document Viewer</title>
            @this.RenderHead(documentViewer)
        </head>
        <body>
            @this.RenderBody(documentViewer)
        </body>
    </html>
    ----------------------------------

    This will render a DocumentViewer control in the page which loads and displays 
    the source document ~/Documents/Document.docx. Upon first view, internally DocumentViewer will 
    convert the source document to PDF (used for "Download as Pdf" and also for next conversion step) 
    and then to XPZ (a special web-friendly format which DocumentViewer uses to actually render documents 
    in the browser). So in this case the user will see "please wait awhile..." message in the viewer 
    for a few seconds. These generated PDF and XPZ files will be cached and upon consecutive page views, 
    the document will be served directly from the cache so the user will see the document instantly 
    on second viewing. When you modify the source document, the cached files are invalidated so 
    your original document and the corresponding cached files are always synced automatically. 
    Note that it's also possible to pre-cache documents via DocumentCache.PreCacheDocument method 
    (e.g. when your user uploads a document), see General Samples for more information.
    
------------------------------------------------------------------------------------------------------
To use DocumentUltimate in an ASP.NET WebForms Project, do the following in Visual Studio:
------------------------------------------------------------------------------------------------------

1. Set DocumentUltimate's global configuration. For example, you may want to set the license key and 
    the cache path. Insert some of the following lines (if overriding a default value is required) into 
    the Application_Start method of your Global.asax.cs:

    ----------------------------------
    //Set this property only if you have a valid license key, otherwise do not
    //set it so DocumentUltimate runs in trial mode.
    DocumentUltimateConfiguration.Current.LicenseKey = "QQJDJLJP34...";

    //The default CacheLocation value is "~/App_Data/DocumentCache"
    //Both virtual and physical paths are allowed (or a Location instance for one of the supported 
    //file systems like Amazon S3 and Azure Blob).
    DocumentUltimateWebConfiguration.Current.CacheLocation = "~/App_Data/DocumentCache";
    ----------------------------------

    Alternatively you can specify the configuration in <appSettings> tag of your Web.config.

    ----------------------------------
    <add key="DocumentUltimate:LicenseKey" value="QQJDJLJP34..." />
    <add key="DocumentUltimateWeb:CacheLocation" value="~/App_Data/DocumentCache" />
    ----------------------------------

    As you would notice, DocumentUltimate: prefix maps to DocumentUltimateConfiguration.Current and 
    DocumentUltimateWeb: prefix maps to DocumentUltimateWebConfiguration.Current.	   
      

2. Open one of your pages (eg. Default.aspx) and at the top of your
    page add add the necessary namespaces:

    ----------------------------------
    <%@ Register TagPrefix="GleamTech" Namespace="GleamTech.DocumentUltimate" Assembly="GleamTech.DocumentUltimate" %>
    <%@ Register TagPrefix="GleamTech" Namespace="GleamTech.DocumentUltimate.AspNet.WebForms" Assembly="GleamTech.DocumentUltimate" %>
    ----------------------------------

    Alternatively you can add the namespaces globally in Web.config under 
    <system.web>/<pages>/<controls> tag to avoid adding namespaces in your pages every time:

    ----------------------------------
    <add tagPrefix="GleamTech" namespace="GleamTech.DocumentUltimate" assembly="GleamTech.DocumentUltimate" />
    <add tagPrefix="GleamTech" namespace="GleamTech.DocumentUltimate.AspNet.WebForms" assembly="GleamTech.DocumentUltimate" />
    ----------------------------------

    Now in your page insert these lines:

    ----------------------------------
    <!DOCTYPE html>
    <html>
        <head runat="server">
            <title>Document Viewer</title>
        </head>
        <body>
            <GleamTech:DocumentViewerControl runat="server" 
                Width="800" 
                Height="600" 
                Document="~/Documents/Document.docx" />
        </body>
    </html>
     ----------------------------------

    This will render a DocumentViewer control in the page which loads and displays 
    the source document ~/Documents/Document.docx. Upon first view, internally DocumentViewer will 
    convert the source document to PDF (used for "Download as Pdf" and also for next conversion step) 
    and then to XPZ (a special web-friendly format which DocumentViewer uses to actually render documents 
    in the browser). So in this case the user will see "please wait awhile..." message in the viewer 
    for a few seconds. These generated PDF and XPZ files will be cached and upon consecutive page views, 
    the document will be served directly from the cache so the user will see the document instantly 
    on second viewing. When you modify the source document, the cached files are invalidated so 
    your original document and the corresponding cached files are always synced automatically. 
    Note that it's also possible to pre-cache documents via DocumentCache.PreCacheDocument method 
    (e.g. when your user uploads a document), see General Samples for more information.
