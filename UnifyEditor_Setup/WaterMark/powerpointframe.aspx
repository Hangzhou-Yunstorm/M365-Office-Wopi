﻿<%-- Copyright (c) Microsoft Corporation. All rights reserved. --%><%@ Assembly Name="Microsoft.Office.Web.Common, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c"%><%@ Register Tagprefix="ppt" Namespace="Microsoft.Office.Server.Powerpoint.Web.UI" Assembly="Microsoft.Office.Server.Powerpoint.Web.UI, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %><%@ Page language="C#" Async="true" AsyncTimeout="20" Codebehind="powerpointframe.aspx.cs" AutoEventWireup="false" EnableViewState="false" Inherits="Microsoft.Office.Server.Powerpoint.Web.UI.PowerPointFrame,Microsoft.Office.Server.Powerpoint.Web.UI,Version=16.0.0.0,Culture=neutral,PublicKeyToken=71e9bce111e9429c" %><!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.1//EN" "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd"><html xmlns:v><head runat="server"><meta http-equiv="X-UA-Compatible" content="IE=99" /><meta HTTP-EQUIV="Content-Type" CONTENT="text/html; charset=utf-8" /><meta HTTP-EQUIV="Expires" content="0" /><script type="text/javascript"> var g_firstByte = new Date(); var g_cssLT; var g_jsLT; var g_bootScriptsStartTime; var g_bootScriptsEndTime; </script><script type="text/javascript"> var WindowVisibilityMetrics;(function(n){function i(){n.metrics={initialVisibility:document.visibilityState,initializeTime:new Date,events:[]};document.addEventListener("visibilitychange",t)}function r(){document.removeEventListener("visibilitychange",t)}function t(){var t={visibility:document.visibilityState,time:new Date};n.metrics.events.push(t)}n.stop=r;i()})(WindowVisibilityMetrics||(WindowVisibilityMetrics={})); </script><style type="text/css"> body { background-color:black; color:white; } </style><style> @media screen and (-ms-high-contrast:active) { div#highContrastDetectionDiv a#highContrastDetectionAnchor { background-image: none !important; } } div#highContrastDetectionDiv, div#screenReaderDetectionDiv { background-color: Window; position: absolute; top: 0px; left: -99999px; width: 1px; height: 1px; opacity: 0; overflow: hidden; } div#highContrastDetectionDiv a#highContrastDetectionAnchor { background-image: url('data:image/png;base64,highContrastDetectorDefault'); } </style></head><body onload="<% if( !StaticResourcesLoader.SpeedupContentLoadEnabled ){ %> onPageLoaded(); <% } %> if(window.adAfterLoad){adAfterLoad();}" scroll="no" <% if ( StaticResourcesLoader.UseTransparentBackground ) { %> style="background-color:transparent;" <% } %> ><% if ( NewBootExperienceEnabled ) { %><![if gte IE 8]><div id="load_back" role="progressbar" aria-label="loadingLabel"><div id="loadingLabel" role="alert" aria-live="assertive" aria-atomic="true" style="overflow: hidden; max-height: 0px; max-width: 0px; z-index:-1;"></div></div><div id="load_img" role="presentation" aria-hidden="true"><div class="load_center"><ppt:NewExperienceLoadingControl id="loader" runat="server" /></div></div><![endif]><% } else { %><% if ( StaticResourcesLoader.SplashScreenMode == SplashMode.WhiteLogo ) { %><![if gte IE 8]><div id="load_back" role="progressbar" aria-label="loadingLabel"><div id="loadingLabel" role="alert" aria-live="assertive" aria-atomic="true" style="overflow: hidden; max-height: 0px; max-width: 0px; z-index:-1;"></div></div><div id="load_img" role="presentation" aria-hidden="true"><div class="load_center"><span role="presentation" align="absmiddle" class="load_logo_ppt_white"></span><br /><Common:SplashscreenLoadingIndicator LegacyIndicator="data:image/gif;base64,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" ID="PowerPoint_white_spinner" Style="white" AppType="PowerPoint" runat="server" /></div></div><![endif]><% } else if ( StaticResourcesLoader.SplashScreenMode == SplashMode.BlackLogo ) { %><![if gte IE 8]><div id="load_back" role="progressbar" aria-label="loadingLabel"><div id="loadingLabel" role="alert" aria-live="assertive" aria-atomic="true" style="overflow: hidden; max-height: 0px; max-width: 0px; z-index:-1;"></div></div><div id="load_img" role="presentation" aria-hidden="true"><div class="load_center"><span role="presentation" align="absmiddle" class="load_logo_ppt_black"></span><br /><Common:SplashscreenLoadingIndicator LegacyIndicator="data:image/gif;base64,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" ID="PowerPoint_dark_spinner" Style="dark" AppType="PowerPoint" runat="server" /></div></div><![endif]><% } %><% } %><div id="applicationOuterContainer" aria-hidden="true" aria-busy="true" style='<%= OuterContainersStyle %>'><ppt:PowerPointStaticResourcesLoader id="StaticResourcesLoader" runat="server" /><% if ( !StaticResourcesLoader.IsEditMode ) { %><script id="default-shader-vs" type="x-shader/x-vertex"> uniform mat4 ModelViewProj; uniform mat4 TransformUV; attribute vec3 aPosition; attribute vec2 aTexcoord; varying vec2 ps_uv; void main(void) { gl_Position = ModelViewProj * vec4( aPosition.x, aPosition.y, aPosition.z, 1.0 ); ps_uv = ( TransformUV * vec4( aTexcoord.x, aTexcoord.y, 0.0, 1.0 ) ).xy; } </script><script id="default-shader-fs" type="x-shader/x-fragment"> precision mediump float; varying vec2 ps_uv; uniform sampler2D uSampler1; uniform float alpha; void main(void) { vec4 texColor = texture2D( uSampler1, vec2( ps_uv.x, 1.0 - ps_uv.y )); vec4 transparent = vec4( 0.0, 0.0, 0.0, 0.0 ); gl_FragColor = mix( transparent, texColor, alpha ); } </script><script id="cc-shader-vs" type="x-shader/x-vertex"> uniform mat4 uModelViewProj; uniform mat4 uTransformUV; attribute vec2 aPosition; attribute vec2 aTexcoord; varying vec2 ps_uv; varying vec2 ps_uv_mask; void main(void) { gl_Position = uModelViewProj * vec4( aPosition.x, aPosition.y, 0.0, 1.0 ); ps_uv_mask = aTexcoord; ps_uv = ( uTransformUV * vec4( aTexcoord.x, aTexcoord.y, 0.0, 1.0 ) ).xy; } </script><script id="cc-shader-fs" type="x-shader/x-fragment"> precision mediump float; varying vec2 ps_uv; varying vec2 ps_uv_mask; uniform sampler2D uSampler1; uniform sampler2D uSampler2; uniform float uAlpha; void main(void) { vec4 texColor = texture2D( uSampler1, vec2( ps_uv.x, 1.0 - ps_uv.y )); vec4 maskColor = texture2D( uSampler2, vec2( ps_uv_mask.x , ps_uv_mask.y ) ); texColor *= maskColor.a; vec4 transparent = vec4( 0.0, 0.0, 0.0, 0.0 ); gl_FragColor = mix( transparent, texColor, uAlpha ); } </script><script id="mask-shader-vs" type="x-shader/x-vertex"> uniform mat4 uModelViewProj; attribute vec2 aPosition; void main(void) { gl_Position = uModelViewProj * vec4( aPosition, 0.0, 1.0 ); } </script><script id="mask-shader-fs" type="x-shader/x-fragment"> precision mediump float; void main(void) { vec4 color = vec4( 0.0, 0.0, 0.0, 1.0 ); gl_FragColor = color; } </script><script id="alpha-shader-vs" type="x-shader/x-vertex"> uniform mat4 uModelViewProj; uniform float uProgress; uniform float uFactor; attribute vec2 aPosition; attribute vec2 aTexcoord; attribute float aDelay; varying vec4 vColor; varying vec2 vTexcoord; void main(void) { float PI = 3.14159265358979323846264; float time = clamp( uFactor * ( uProgress - aDelay ), 0.0, 1.0); time = 1.0 - 0.5 * ( 1.0 + cos( time * PI ) ); gl_Position = uModelViewProj * vec4( aPosition, 0.0, 1.0 ) ; vTexcoord = aTexcoord; vColor = vec4( time ); } </script><script id="alpha-shader-fs" type="x-shader/x-fragment"> precision mediump float; uniform float uStyle; varying vec4 vColor; varying vec2 vTexcoord; uniform sampler2D uSampler1; uniform sampler2D uSampler2; float when_eq( float x, float y ) { return 1.0 - abs( sign( x - y ) ); } void main(void) { gl_FragColor = when_eq( uStyle, 1.0 ) * mix( texture2D( uSampler1, vTexcoord ), texture2D( uSampler2, vTexcoord ), vColor.a ) + when_eq( uStyle, 2.0 ) * ( texture2D( uSampler1, vTexcoord ) * vColor.a ); } </script><script id="morph-shader-fade-vs" type="x-shader/x-vertex"> uniform mat4 ModelViewProj; uniform mat4 TransformUVA; uniform float uProgress; attribute vec2 aPosition; attribute vec2 aTexcoord; varying vec2 ps_uvA; varying float ps_progress; void main(void) { float t = clamp( uProgress, 0.0, 1.0 ); gl_Position = ModelViewProj * vec4( aPosition.x, aPosition.y, 0.0, 1.0 ); ps_progress = t; ps_uvA = ( TransformUVA * vec4( aTexcoord.x, aTexcoord.y, 0.0, 1.0 ) ).xy; } </script><script id="morph-shader-crossfade-vs" type="x-shader/x-vertex"> uniform mat4 ModelViewProj; uniform mat4 TransformUVA; uniform mat4 TransformUVB; uniform float uProgress; uniform float uUseGrid; attribute vec3 aPosition; attribute vec2 aTexcoord; attribute vec2 aGridA; attribute vec2 aGridB; varying vec2 ps_uvA; varying vec2 ps_uvB; varying float ps_progress; float when_eq(float x, float y) { return 1.0 - abs(sign(x - y)); } void main(void) { float t = clamp( uProgress, 0.0, 1.0 ); vec2 uv = vec2( aTexcoord.x, 1.0-aTexcoord.y ); vec2 uvA = uv + when_eq( uUseGrid, 1.0 ) * aGridA; vec2 uvB = uv + when_eq( uUseGrid, 1.0 ) * aGridB; uv = mix( uvA, uvB, t ); vec2 pos = vec2( aPosition.x, 1.0 - aPosition.y ); vec2 posA = pos + when_eq( uUseGrid, 1.0 ) * aGridA; vec2 posB = pos + when_eq( uUseGrid, 1.0 ) * aGridB; pos = mix( posA, posB, t ); vec4 posMesh = vec4( pos, aPosition.z, 1.0 ); gl_Position = ModelViewProj * posMesh; ps_progress = t; ps_uvA = ( TransformUVA * vec4( uvA, 0.0, 1.0 ) ).xy; ps_uvB = ( TransformUVB * vec4( uvB, 0.0, 1.0 ) ).xy; } </script><script id="morph-shader-fade-fs" type="x-shader/x-fragment"> precision mediump float; varying vec2 ps_uvA; varying float ps_progress; uniform sampler2D uSampler1; uniform vec4 uvClippingA; float when_gt(float x, float y) { return max(sign(x - y), 0.0); } float when_lt(float x, float y) { return max(sign(y - x), 0.0); } vec4 clipTexture( vec2 uv, sampler2D tex, vec4 uvClipping ) { float fClipped = sign( when_lt( uv.x, uvClipping[0] ) + when_gt( uv.x, uvClipping[2] ) + when_lt( uv.y, uvClipping[1] ) + when_gt( uv.y, uvClipping[3] ) ); return vec4(1.0 - fClipped) * texture2D( tex, vec2( uv.x, 1.0 - uv.y ) ); } void main(void) { vec4 transparent = vec4( 0.0, 0.0, 0.0, 0.0 ); vec4 colorA = transparent; vec4 colorB = transparent; colorA = clipTexture( ps_uvA, uSampler1, uvClippingA ); vec4 outcolor = mix( colorA, colorB, ps_progress ); gl_FragColor = outcolor; } </script><script id="morph-shader-crossfade-fs" type="x-shader/x-fragment"> precision mediump float; varying vec2 ps_uvA; varying vec2 ps_uvB; varying float ps_progress; varying vec2 ps_uvDebugMode; uniform sampler2D uSampler1; uniform sampler2D uSampler2; uniform vec4 uvClippingA; uniform vec4 uvClippingB; float when_gt(float x, float y) { return max(sign(x - y), 0.0); } float when_lt(float x, float y) { return max(sign(y - x), 0.0); } vec4 clipTexture( vec2 uv, sampler2D tex, vec4 uvClipping ) { float fClipped = sign( when_lt( uv.x, uvClipping[0] ) + when_gt( uv.x, uvClipping[2] ) + when_lt( uv.y, uvClipping[1] ) + when_gt( uv.y, uvClipping[3] ) ); return vec4(1.0 - fClipped) * texture2D( tex, vec2( uv.x, 1.0 - uv.y ) ); } void main(void) { vec4 transparent = vec4( 0.0, 0.0, 0.0, 0.0 ); vec4 colorA = transparent; vec4 colorB = transparent; colorA = clipTexture( ps_uvA, uSampler1, uvClippingA ); colorB = clipTexture( ps_uvB, uSampler2, uvClippingB ); vec4 outcolor = mix( colorA, colorB, ps_progress ); gl_FragColor = outcolor; } </script><script id="morph-shader-bg-fs" type="x-shader/x-fragment"> precision mediump float; varying vec2 ps_uvA; varying vec2 ps_uvB; varying float ps_progress; uniform sampler2D uSampler1; uniform sampler2D uSampler2; uniform vec4 uvClippingA; uniform vec4 uvClippingB; uniform vec4 bgSlideA; uniform vec4 bgSlideB; float when_gt(float x, float y) { return max(sign(x - y), 0.0); } float when_lt(float x, float y) { return max(sign(y - x), 0.0); } vec4 when_neq(vec4 x, vec4 y) { return abs(sign(x - y)); } vec4 when_eq(vec4 x, vec4 y) { return 1.0 - abs(sign(x - y)); } vec4 clipTexture( vec2 uv, sampler2D tex, vec4 uvClipping ) { float fClipped = sign( when_lt( uv.x, uvClipping[0] ) + when_gt( uv.x, uvClipping[2] ) + when_lt( uv.y, uvClipping[1] ) + when_gt( uv.y, uvClipping[3] ) ); return vec4(1.0 - fClipped) * texture2D( tex, vec2( uv.x, 1.0 - uv.y ) ); } void main(void) { vec4 transparent = vec4( 0.0, 0.0, 0.0, 0.0 ); vec4 colorA = transparent; vec4 colorB = transparent; vec4 fSolidBGA = when_neq( bgSlideA, transparent ); vec4 fSolidBGB = when_neq( bgSlideB, transparent ); colorA = clipTexture( ps_uvA, uSampler1, uvClippingA ) * (vec4(1.0) - fSolidBGA) + bgSlideA * fSolidBGA; colorB = clipTexture( ps_uvB, uSampler2, uvClippingB ) * (vec4(1.0) - fSolidBGB) + bgSlideB * fSolidBGB; vec4 outcolor = mix( colorA, colorB, ps_progress ); gl_FragColor = outcolor; } </script><% } %><% if ( !StaticResourcesLoader.IsEditMode ) { %><script type="text/javascript"> function ServiceError(){}function ViewPreview(n){var t;this.$$d_$F_0=Function.createDelegate(this,this.$F_0);this.$$d_$L_0=Function.createDelegate(this,this.$L_0);this.$$d_$K_0=Function.createDelegate(this,this.$K_0);try{this.$1_0=n;var i=new VP.FormatPicker(this.$1_0.FormatInfos,!0,this.$1_0.AspectRatio),r=VP.ViewPreviewHelper.getAvailableDimensions(this.$1_0.View,this.$1_0.IsEmbedded,this.$1_0.AddShadowBorder,this.$1_0.ToolbarWithTopbarHeight),u=VP.ViewPreviewHelper.getMinFormat(this.$1_0.View,i);this.$D_0=i.getFormatInfo(r,u,this.$J_0());t=new Sys.Net.WebRequest;t.set_url(this.$1_0.PreviewImageUrl);t.set_body(String.format(this.$1_0.PreviewImageBody,this.$D_0.Format.toString()));t.get_headers()["Content-Type"]="application/json; charset=utf-8";this.$1_0.Datacenter&&(t.get_headers()["X-WacCluster"]=this.$1_0.Datacenter);t.add_completed(this.$$d_$K_0);t.invoke()}catch(f){}}function ViewPreviewSettings(){}Type.registerNamespace("VP");VP.PowerpointView=function(){};VP.PowerpointView.prototype={ReadingView:0,SlideShowView:1,EditView:2,StaticView:3,OutlineView:4,AttendeeView:5,ChromelessView:6};VP.PowerpointView.registerEnum("VP.PowerpointView",!1);VP.PowerPointFormat=function(){};VP.PowerPointFormat.prototype={AnimatedMedium:0,AnimatedLarge:1,AnimatedSmall:2,AnimatedExtraSmall:3,AnimatedExtraLarge:4,Media:5,AnimatedExtraExtraLarge:6,StaticImage:7};VP.PowerPointFormat.registerEnum("VP.PowerPointFormat",!1);VP.PowerPointStaticFormat=function(){};VP.PowerPointStaticFormat.prototype={StaticMedium:0,StaticLarge:1,StaticSmall:2};VP.PowerPointStaticFormat.registerEnum("VP.PowerPointStaticFormat",!1);ViewPreview.prototype={dispose:function(){this.$0_0&&(this.$5_0&&($removeHandler(this.$0_0,"load",this.$5_0),this.$5_0=null),this.$6_0&&($removeHandler(window,"resize",this.$6_0),this.$6_0=null),this.$0_0=null)},$K_0:function(n){var t,r,i;try{if(t=Sys.Browser.agent===Sys.Browser.InternetExplorer&&window.document.documentMode===8,n.get_statusCode()!==200||!n.get_responseData()||!this.$1_0.IsShowable||t){r=String.format("OnGetPreviewCompleted:Not generating preview image. Executor.StatusCode: {0}; executor.ResponseData: {1}; m_settings.IsShowable: {2}; isIE8: {3}",n.get_statusCode(),!n.get_responseData(),this.$1_0.IsShowable,t);VP.ViewPreviewHelper.log(r);return}if(i=Sys.Serialization.JavaScriptSerializer.deserialize(n.get_responseData()),!i.Result){VP.ViewPreviewHelper.log("OnGetPreviewCompleted:serviceResult is null");return}this.$0_0=document.createElement("img");this.$0_0.id="preview";this.$0_0.style.visibility="hidden";this.$0_0.style.position="absolute";this.$0_0.style.zIndex=1;this.$5_0=this.$$d_$L_0;$addHandler(this.$0_0,"load",this.$5_0);this.$0_0.src=i.Result;document.body.appendChild(this.$0_0);VP.ViewPreviewHelper.log("OnGetPreviewCompleted:Generated preview image")}catch(u){}},$L_0:function(){try{if(this.$0_0.width===1&&this.$0_0.height===1){VP.ViewPreviewHelper.log("OnLoad:Slide has content which is incompatible for preview optimization");return}this.$B_0=this.$0_0.width;this.$A_0=this.$0_0.height;this.$0_0.setAttribute("render",(new Date).getTime().toString());this.$F_0(null);this.$6_0=this.$$d_$F_0;$addHandler(window,"resize",this.$6_0)}catch(n){}},$F_0:function(){var r,u;try{var n=VP.ViewPreviewHelper.getAvailableDimensions(this.$1_0.View,this.$1_0.IsEmbedded,this.$1_0.AddShadowBorder,this.$1_0.ToolbarWithTopbarHeight),t=VP.ViewPreviewHelper.getTopLeftForPreview(this.$1_0.View,this.$1_0.IsEmbedded,this.$1_0.AddShadowBorder,this.$1_0.ToolbarWithTopbarHeight),i=this.$H_0(n);this.$0_0.style.width=Math.floor(this.$B_0*i)+"px";this.$0_0.style.height=Math.floor(this.$A_0*i)+"px";this.$0_0.style.left=Math.floor(t.x+(n.$3_0-this.$0_0.width)/2)+"px";this.$0_0.style.top=Math.floor(t.y+(n.$2_0-this.$0_0.height)/2)+"px";this.$0_0.setAttribute("loaded","y");this.$0_0.style.visibility="visible";r=document.getElementById("load_back");r.style.display="none";u=document.getElementById("load_img");u.style.display="none"}catch(f){}},$H_0:function(n){var i=n.$3_0/this.$B_0,r=n.$2_0/this.$A_0,t=Math.min(i,r);return t>1?1:t},$J_0:function(){var n=window.devicePixelRatio;return isNaN(n)?window.screen.deviceXDPI/window.screen.logicalXDPI:n},$0_0:null,$B_0:0,$A_0:0,$6_0:null,$5_0:null,$D_0:null,$1_0:null};VP.ViewPreviewHelper=function(){};VP.ViewPreviewHelper.getAvailableDimensions=function(n,t,i,r){var u;switch(n){case 6:u=VP.BrowserHelperSlim.getClientDimensions(300,201);t&&(u.$3_0-=2,u.$2_0-=2,u.$2_0-=23);i&&(u.$3_0-=20,u.$2_0-=20);break;case 1:u=VP.BrowserHelperSlim.getClientDimensions(300,200);break;case 0:default:u=VP.BrowserHelperSlim.getClientDimensions(300,200);u.$3_0-=20;u.$2_0-=20;u.$2_0-=r;u.$2_0-=23}return u};VP.ViewPreviewHelper.getTopLeftForPreview=function(n,t,i,r){var u=new Sys.UI.Point(0,0);switch(n){case 6:t&&(u.x+=1,u.y+=1);i&&(u.x+=10,u.y+=10);break;case 1:break;case 0:default:u.x+=10;u.y+=10;u.y+=r}return u};VP.ViewPreviewHelper.getMinFormat=function(n,t){switch(n){case 6:return t.get_minPowerPointFormat();case 1:case 0:default:return 0}};VP.ViewPreviewHelper.log=function(n){VP.ViewPreviewHelper.$I();try{VP.ViewPreviewHelper.$4.innerText+=n+"; "}catch(t){}};VP.ViewPreviewHelper.$I=function(){VP.ViewPreviewHelper.$4||(VP.ViewPreviewHelper.$4=document.createElement("div"),VP.ViewPreviewHelper.$4.id="PreviewLogger",VP.ViewPreviewHelper.$4.style.display="none",document.body.appendChild(VP.ViewPreviewHelper.$4))};VP.Constants=function(){};VP.PowerPointFormatInfo=function(n,t,i){this.Format=n;this.SlideWidth=t;this.SlideHeight=i};VP.PowerPointFormatInfo.prototype={Format:0,SlideWidth:0,SlideHeight:0};VP.PowerPointFormatInfoHelper=function(){};VP.PowerPointFormatInfoHelper.pickFormatInfo=function(n,t,i,r,u,f,e){var h,c,o,s,l,a;if(!n)throw Error.argumentNull("powerPointFormatInfosLargestToSmallest");for(o=null,s=0;s<n.length;s++)if(o=n[s],u?(VP.PowerPointFormatInfoHelper.getWidthAndHeightUsingAspectRatio(o,u,l={val:h},a={val:c}),h=l.val,c=a.val):(h=o.SlideWidth,c=o.SlideHeight),f!==1&&(t=Math.round(t*f),i=Math.round(i*f)),h<=t&&c<=i||o.Format===e||s===n.length-1){h/t<.9&&c/i<.9&&r&&s&&(o=n[s-1]);break}return o};VP.PowerPointFormatInfoHelper.getWidthAndHeightUsingAspectRatio=function(n,t,i,r){if(!n)throw Error.argumentNull("formatInfo");VP.PowerPointFormatInfoHelper.getSizeUsingAspectRatio(n.SlideWidth,n.SlideHeight,t,i,r)};VP.PowerPointFormatInfoHelper.getSizeUsingAspectRatio=function(n,t,i,r,u){r.val=n;u.val=t;var f=r.val/u.val;f!==i&&(f>i?r.val=Math.round(u.val*i):u.val=Math.round(r.val/i))};ViewPreviewSettings.prototype={View:0,FormatInfos:null,PreviewImageUrl:null,PreviewImageBody:null,IsShowable:!1,IsEmbedded:!1,AddShadowBorder:!1,ToolbarWithTopbarHeight:0,Datacenter:null,AspectRatio:0};VP.Dimensions=function(n,t){this.$3_0=n;this.$2_0=t};VP.Dimensions.prototype={$3_0:0,$2_0:0,get_width:function(){return this.$3_0},set_width:function(n){return this.$3_0=n,n},get_height:function(){return this.$2_0},set_height:function(n){return this.$2_0=n,n},isLinearlySmallerThanOrEqualTo:function(n){if(!n)throw Error.argumentNull();return this.$3_0<=n.$3_0&&this.$2_0<=n.$2_0},equals:function(n){return this.$3_0===n.$3_0&&this.$2_0===n.$2_0},toCompatibleObject:function(){var n={};return n.width=this.$3_0,n.height=this.$2_0,n}};VP.FormatPicker=function(n,t,i){this.$7_0=n;this.$8_0={};for(var r=0;r<n.length;r++)this.$8_0[n[r].Format.toString()]=r;this.$C_0=t;this.$9_0=i&&!isNaN(i)&&isFinite(i)?i:0};VP.FormatPicker.prototype={getFormatIndex:function(n,t,i){var r=this.getFormatInfo(n,t,i);return r?this.$8_0[r.Format.toString()]:this.$8_0["0"]},getFormatInfo:function(n,t,i){return VP.PowerPointFormatInfoHelper.pickFormatInfo(this.$7_0,n.$3_0,n.$2_0,this.$C_0,this.$9_0,i,t)},get_minPowerPointFormat:function(){return this.$7_0?this.$7_0[this.$7_0.length-1].Format:0},$7_0:null,$C_0:!1,$9_0:null,$8_0:null};VP.DataMemberAttribute=function(){VP.DataMemberAttribute.initializeBase(this)};VP.DataContractAttribute=function(){VP.DataContractAttribute.initializeBase(this)};VP.BrowserHelperSlim=function(){};VP.BrowserHelperSlim.getClientDimensions=function(n,t){var i=Math.max(window.document.documentElement.clientWidth,n),r=Math.max(window.document.documentElement.clientHeight,t);return new VP.Dimensions(i,r)};VP.BrowserHelperSlim.getScreenDimensions=function(){return new VP.Dimensions(window.screen.width,window.screen.height)};VP.BrowserHelperSlim.getClientDevicePixelRatio=function(){var n=window.devicePixelRatio;return isNaN(n)&&(n=window.screen.deviceXDPI/window.screen.logicalXDPI,isNaN(n)&&(n=1)),n};ServiceError.registerClass("ServiceError");ViewPreview.registerClass("ViewPreview");VP.ViewPreviewHelper.registerClass("VP.ViewPreviewHelper");VP.Constants.registerClass("VP.Constants");VP.PowerPointFormatInfo.registerClass("VP.PowerPointFormatInfo");VP.PowerPointFormatInfoHelper.registerClass("VP.PowerPointFormatInfoHelper");ViewPreviewSettings.registerClass("ViewPreviewSettings");VP.Dimensions.registerClass("VP.Dimensions");VP.FormatPicker.registerClass("VP.FormatPicker");VP.DataMemberAttribute.registerClass("VP.DataMemberAttribute",Sys.Attribute);VP.DataContractAttribute.registerClass("VP.DataContractAttribute",Sys.Attribute);VP.BrowserHelperSlim.registerClass("VP.BrowserHelperSlim");VP.ViewPreviewHelper.divTag="div";VP.ViewPreviewHelper.$4=null;VP.Constants.unifiedHeaderHeight=50;VP.Constants.editDefaultRibbonHeight=30;VP.Constants.editLowerRibbonHeightMousePx=94;VP.Constants.editLowerRibbonHeightTouchPx=108;VP.Constants.editLowerRibbonHeightSingleRowLayoutPx=48;VP.Constants.statusBarHeight=23;VP.Constants.slidePadding=10;VP.Constants.defaultScrollbarSize=17;VP.Constants.editAreaMinimumWidth=756;VP.Constants.editAreaMinimumHeight=513;VP.Constants.editNotesHeightPeek=32;VP.Constants.maxMinimalBootWaitingTime=23;VP.Constants.chromelessMinWidth=300;VP.Constants.chromelessMinHeight=201;VP.Constants.minWidth=300;VP.Constants.minHeight=200;VP.Constants.outlineDefaultToolbarHeight=70;VP.Constants.embedBorderWidth=1;VP.Constants.notesPadding=6;VP.Constants.fullscreenSlidePadding=0;VP.Constants.contextMenuPadding=5;VP.Constants.slideBorderWidth=1;VP.Constants.slideMenuOffset=26;VP.Constants.viewportBufferSize=2;VP.Constants.maxShapesOnSlide=1e5;VP.Constants.shapeBorderColor="#a6a6a6";VP.Constants.shapeBorderWidth=1;VP.Constants.shapeBoundsToTextBounds=10;VP.Constants.lineSelectionBoxPaddingTouch=10;VP.Constants.lineSelectionBoxPaddingClick=5;VP.Constants.shapePresenceBorderWidth=3;VP.Constants.selectionBoxHollowAreaColor="white";VP.Constants.selectionBoxHollowAreaWidth=3;VP.Constants.draggableChromeSelectionBoxDragFill="rgba(255,255,255,0.5)";VP.Constants.selectionBoxInnerBorderWidth=2;VP.Constants.seletionBoxBorderWidth=6;VP.Constants.shapeInnerDraggableAreaWidth=2;VP.Constants.selectionBoxExtendedWidth=6;VP.Constants.shapeSelectionElementZOrderBase=250;VP.Constants.progressSpinnerHeight=24;VP.Constants.progressSpinnerWidth=24;VP.Constants.subtleProgressIndicatorOpacity=0;VP.Constants.strongProgressIndicatorOpacity=.5;VP.Constants.editAreaMinimumHeightForFlexBox=463;VP.Constants.editNotesHeightShow=100;VP.Constants.editNotesHeightHide=3;VP.Constants.editNotesHeightMinHeight=10;VP.Constants.minShapeWidth=110;VP.Constants.minShapeHeight=75;VP.Constants.editRibbonDataVariable="PowerPointEditingRibbonData";VP.Constants.editRibbonMinimizedCookieName="WACRibbonMinimizedPPT";VP.Constants.editRibbonClientID="ribbon";VP.Constants.homeTab="PptRibbon.Home";VP.Constants.smartArtTab="PptRibbon.SmartArt";VP.Constants.drawingFormatTab="PptRibbon.DrawingFormat";VP.Constants.pictureFormatTab="PptRibbon.PictureFormat";VP.Constants.reviewTab="PptRibbon.Review";VP.Constants.reactRibbonHomeTab="Home";VP.Constants.reactRibbonSmartArtTab="SmartArtDesign";VP.Constants.reactRibbonDrawingFormatTab="FormatShape";VP.Constants.reactRibbonPictureFormatTab="FormatPicture";VP.Constants.reactRibbonReviewTab="Review";VP.Constants.reactRibbonHeight=73;VP.Constants.basicAnimationClass="Shared_BasicClass";VP.Constants.basicSlideBorderClass="BasicSlideBorder";VP.Constants.fancySlideBorderClass="FancySlideBorder";VP.Constants.shadowClasses=["FShadowRight","FShadowBottom","FShadowTop","FShadowLeft","FShadowRight2","FShadowBottom2","FShadowTop2","FShadowLeft2","FShadowTopRight","FShadowBottomRight","FShadowBottomLeft","FShadowTopLeft","FShadowTopRight2","FShadowBottomRight2","FShadowBottomLeft2","FShadowTopLeft2"];VP.Constants.editContextMenuId="editContextMenu";VP.Constants.previewImageId="preview";VP.Constants.previewImageLoadedAttribute="loaded";VP.Constants.previewImageUsedAttribute="used";VP.Constants.previewImageRenderTimeAttribute="render";VP.Constants.designerPaneClass="DesignerPane";VP.Constants.designerPaneClassRTL="DesignerPaneRTL";VP.Constants.designerPaneClassLTR="DesignerPaneLTR";VP.Constants.designerPaneWidth=263;VP.Constants.designerPaneWidthIdeas=304;VP.Constants.designerPaneBorder=1;VP.Constants.designerHelpLink="https://go.microsoft.com/fwlink/?linkid=849683";VP.Constants.hideInactiveAreaDuringBoot="HideDuringBoot";VP.Constants.taskPaneBackground="TaskPaneBackgroundColor";VP.Constants.taskPaneBorder="TaskPaneBorder";VP.Constants.pictureCropTaskPaneId="PictureCropTaskPane";VP.Constants.pictureCropTaskPaneDataSource="PictureCropTaskPaneData";VP.Constants.pictureCropSectionId="CropSection";VP.Constants.pictureCropTaskPaneWidth=279;VP.Constants.pictureCropTaskPaneBorder=1;VP.Constants.tableTaskPaneId="TableTaskPane";VP.Constants.tableTaskPaneDataSource="TableTaskPaneData";VP.Constants.tableSizeSectionId="TableSizeSection";VP.Constants.cellSizeSectionId="CellSizeSection";VP.Constants.tableTaskPaneWidth=279;VP.Constants.tableTaskPaneBorder=1;VP.Constants.imageDescriptionPaneClass="ImageDescriptionPane";VP.Constants.imageDescriptionPaneClassRTL="ImageDescriptionPaneRTL";VP.Constants.imageDescriptionPaneClassLTR="ImageDescriptionPaneLTR";VP.Constants.imageDescriptionPaneWidth=263;VP.Constants.imageDescriptionPaneBorder=1;VP.Constants.pictureCropPaneClass="PictureCropPane";VP.Constants.pictureCropPaneClassRTL="PictureCropPaneRTL";VP.Constants.pictureCropPaneClassLTR="PictureCropPaneLTR";VP.Constants.pictureCropPaneWidth=263;VP.Constants.pictureCropPaneBorder=1;VP.Constants.documentPanelContentClass="WACDocumentPanelContent";VP.Constants.documentPanelContentFocusClass="WACDocumentPanelContentFocused";VP.Constants.documentPanelContentFishBowlClass="FishBowlPanel";VP.Constants.documentPanelContentEditModeClass="EditMode";VP.Constants.animationMarkerElementId="AnimationMarkerElement";VP.Constants.animationMarkerClassName="animMarkerHovered"; </script><% } else if ( StaticResourcesLoader.IsApollo ) { %><script type="text/javascript"> function EarlyGetPresRequestGetter(n){this.$$d_$A=Function.createDelegate(this,this.$A);this.$2=new XMLHttpRequest;try{if(!window.JSON)return;if(!n.PresentationId)return;this.$2.open("POST",n.WebServiceBase+n.ServiceEndpoint+"/GetBootEditPresInfo?openEarly=true",!0);this.$2.onreadystatechange=this.$$d_$A;this.$2.setRequestHeader("Content-Type","application/json; charset=utf-8");this.$2.setRequestHeader(EP.$0.$4,n.SessionInfo);this.$2.setRequestHeader("X-xhr","1");n.SessionId&&this.$2.setRequestHeader("X-UserSessionId",n.SessionId);n.AccessToken&&this.$2.setRequestHeader("X-AccessToken",n.AccessToken);n.BuildVersion&&this.$2.setRequestHeader("X-OfficeVersion",n.BuildVersion);n.Canary&&this.$2.setRequestHeader("X-Key",n.Canary);n.UserType&&this.$2.setRequestHeader("X-UserType",n.UserType);n.Datacenter&&this.$2.setRequestHeader("X-WacCluster",n.Datacenter);var u=EP.$3.$9(0,0),t=EP.$2.$7(u,756,513,17),r=Math.max(t.$1,756),i=Math.max(t.$0,513);i-=30;i-=94;i-=23;i-=32;r-=256;t=EP.$2.$8(new EP.$1(r,i),new EP.$1(n.BaseWidth,n.BaseHeight));n.IsZoomEnabled&&(this.$2.setRequestHeader("SlideWidth",t.$1.toString()),this.$2.setRequestHeader("SlideHeight",t.$0.toString()));var f=EarlyGetPresRequestGetter.$5(n.PresentationId),e=EarlyGetPresRequestGetter.$5(n.CurrentSlideId?-1:0),o=EarlyGetPresRequestGetter.$5(n.CurrentSlideId.toString()),s='{"presentationId":'+f+',"firstSlideIndex":0,"numSlides":1,"includedEditSlideIndex":'+e+',"includedSlideId":'+o+',"clientWidth":'+t.$1+',"clientHeight":'+t.$0+"}";this.$2.send(s)}catch(h){}}function $4(){}Type.registerNamespace("EP");EP.$2=function(){};EP.$2.$7=function(n,t,i,r){var u=new EP.$1(n.$1,n.$0),f=u.$1<t,e;return f&&(u.$0-=r),e=u.$0<i,e&&(u.$1-=r),!f&&u.$1<t&&(f=!0,u.$0-=r),u};EP.$2.$8=function(n,t){var i=new EP.$1(n.$1-20,n.$0-20),r=new EP.$1(t.$1/2,t.$0/2),u=new EP.$1(t.$1*2,t.$0*2);return i.$1<r.$1?i.$1=r.$1:i.$1>u.$1&&(i.$1=u.$1),i.$0<r.$0?i.$0=r.$0:i.$0>u.$0&&(i.$0=u.$0),i};EarlyGetPresRequestGetter.$5=function(n){return JSON.stringify(n)};EarlyGetPresRequestGetter.prototype={$A:function(){var i,n,t,r;this.$2.readyState===4&&(i=window.self,this.$2.status===200?(n=Sys.Serialization.JavaScriptSerializer.deserialize(this.$2.responseText),t={},t[EP.$0.$4]=this.$2.getResponseHeader(EP.$0.$4),t[EP.$0.$6]=this.$2.getResponseHeader(EP.$0.$6),n.ResponseHeaders=t):(n={},n.IsOpenEarlyRequestFailed=!0),r=i.ApolloLoaded,r?r(n):i.PreloadedPresInfo=n)}};EP.$1=function(n,t){this.$1=n;this.$0=t};EP.$1.prototype={$1:0,$0:0};EP.$0=function(){};EP.$3=function(){};EP.$3.$9=function(n,t){var i=Math.max(window.document.documentElement.clientWidth,n),r=Math.max(window.document.documentElement.clientHeight,t);return new EP.$1(i,r)};$4.prototype={PresentationId:null,ServiceEndpoint:null,AccessToken:null,AccessTokenTtl:0,SessionId:null,BuildVersion:null,WebServiceBase:null,Canary:null,UserType:null,SessionInfo:null,BaseWidth:0,BaseHeight:0,Datacenter:null,IsZoomEnabled:!1,CurrentSlideId:0,IsHighContrastEnabled:!1,MaxCharsPerRequestBatch:0,IsPreFetchImagesForOpenEarlyEnabled:!1,IsPodsEnabled:!1,PodSID:null,ImageMaxDownloadingThread:0,ZeroBytePostEnabled:!1,IsDedicatedEarlyImageRequestEnabled:!1,IsHighDpiSupportEnabled:!1,IsSimplifiedRibbonOn:!1,IsMultilineRibbonMinimized:!1};EP.$2.registerClass("EP.$2");EarlyGetPresRequestGetter.registerClass("EarlyGetPresRequestGetter");EP.$1.registerClass("EP.$1");EP.$0.registerClass("EP.$0");EP.$3.registerClass("EP.$3");$4.registerClass("$4");EP.$0.$4="si";EP.$0.$6="esid"; </script><% } %><form runat="server" action="/"><div id="ApplicationContainer" runat="server"></div><Common:KomodoBridge DataProcessors="Common_Logs; Box4_GraphNodes" id="KmdToolContainer" runat="server" /></form>
<script src="App_Scripts/jquery.min.js"></script>
<script src="App_Scripts/PPTWaterMark.js?v=0721"></script>
</div></body></html> 
