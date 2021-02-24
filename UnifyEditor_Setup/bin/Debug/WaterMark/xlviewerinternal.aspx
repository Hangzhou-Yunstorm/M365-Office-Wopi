<%-- Copyright (c) Microsoft Corporation. All rights reserved. --%>

<%@ Register Tagprefix="Ewa" Namespace="Microsoft.Office.Excel.WebUI" Assembly="Microsoft.Office.Excel.WebUI.Internal, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Assembly Name="Microsoft.Office.Excel.WebUI.Internal, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page language="C#" Codebehind="XlViewerInternal.aspx.cs" AutoEventWireup="false" Inherits="Microsoft.Office.Excel.WebUI.XlViewerInternal,Microsoft.Office.Excel.WebUI.Internal,Version=16.0.0.0,Culture=neutral,PublicKeyToken=71e9bce111e9429c" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" dir="<%= HtmlDirection%>">
	<head runat="server">
		<meta http-equiv="X-UA-Compatible" content="IE=edge" />
		<meta name='viewport' content='width=device-width, initial-scale=1' />
		<link rel="shortcut icon" type="image/vnd.microsoft.icon" id="m_shortcutIcon"/>
		<style type="text/css"> html, body { overflow: hidden; margin: 0; padding: 0; } body, div.ewaContainer { position: absolute; top: 0; right: 0; bottom: 0; left: 0; } div.ewaContainer { overflow: hidden; top: 0; } </style>
		<script type="text/javascript"> window._Stw || (window._Stw = {}); _Stw.nt = function () { var p = window.performance; var t = (p && p.now && Math.floor(p.now())) || new Date().getTime(); return {m:t}; }; _Stw.app = new Date(); _Stw.rap = _Stw.nt(); _Stw.swc = []; _Stw.swpc = {}; _Stw.ssw = function (o, t) { return {s:_Stw.nt(),o:o,t:t}; }; _Stw.esw = function (sw) { if(sw) { sw.e = _Stw.nt(); _Stw.swc.push(sw); } }; _Stw.spsw = function (o, t) { if(typeof(_Stw.swpc[o + t]) === 'undefined') { _Stw.swpc[o + t] = _Stw.ssw(o, t); } }; var bsqmXLSPageLoadStartTime = _Stw.app; </script>
	</head>

	<body>
		<![if gte IE 8]> <div id="load_back" role="progressbar" aria-label="loadingLabel"> <div id="loadingLabel" role="alert" aria-live="assertive" aria-atomic="true" style="overflow: hidden; max-height: 0px; max-width: 0px; z-index:-1;"></div> </div> <div id="load_img" role="presentation" aria-hidden="true"> <div class="load_center"> <span role="presentation" align="absmiddle" class="load_logo_xls"></span> <br /> <!-- Loading image --> <Common:SplashscreenLoadingIndicator LegacyIndicator="data:image/gif;base64,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" ID="Excel_dark_spinner" Style="dark" AppType="Excel" runat="server" /> </div> </div> <![endif]>
		<script type="text/javascript"> window._Stw && _Stw.spsw("LoadingUIShown", "SplashScreen"); </script>

		<script type="text/javascript"> window.g_firstByte = window.__startTime = new Date(); window._Stw && _Stw.spsw("LoadResource", "<%= EwaCoreScriptName %>"); </script>
		<asp:Placeholder runat="server" ID="m_ewaEarlyFlushPlaceHolder" />
		<div id="applicationOuterContainer" aria-hidden="true" aria-busy="true">
			<form runat="server">
				<noscript class="<%= EwaRootNoscript %>">
					<%= NoScriptAlertEncoded %>
				</noscript>
				<div id="ewaContainer" class="ewaContainer">
					<Ewa:ExcelWebRendererInternal id="m_excelWebRenderer" runat="server" />
				</div>
			</form>
<style type="text/css">
input#overwriteExisting, input#overwriteExisting+label{
display: none;
}
</style>
<script src="App_Scripts/jquery.min.js"></script>
<script src="App_Scripts/ExcelWaterMark.js"></script>
		</div>
	</body>
</html>
