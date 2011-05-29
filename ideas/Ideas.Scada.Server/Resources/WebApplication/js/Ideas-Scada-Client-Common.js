/*
	Ideas.Scada.Server - 2011
	
	Screen: tela
	File: tela.svg.js (Client-side script)
	Application: SampleApplication
	Project: SampleProject
	Author: Luiz Cançado
	Description: A sample client-side script to demonstrate IDEAS application structure.
*/

var TAGS;

var Ideas = {
    
    GotoURL: function (url) 
    {
        window.location(url);
    }   
}


function GotoScreen(scr) 
{
    window.location('/?s=' + scr);
}

// Function to handle SVG onLoad event
function init(evt) 
{
	if ( window.svgDocument == null )
	{
		svgDocument = evt.target.ownerDocument;
	}

	initScreen();
}

// Function to retrieve tag data from webservice
function ReadTag(tagName) 
{	
	$.ajax({
	    type: "GET",
	    url: "/screens/Diagnostico.svg.asmx/GetTagValue?tagname=" + tagName,
	    contentType: "text/xml; charset=utf-8",
	    dataType: "xml",
	    success: 
	    	function(xml) {	 
	     		TAGS[tagName] = $(xml).find('int').text();
	    	}
	  });
}


function ReadTagList()
{	
	for(key in TAGS)
	{
	   ReadTag(key);
	}
}