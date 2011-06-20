/*
	Ideas.Scada.Server - 2011
	
	Screen: tela
	File: tela.svg.js (Client-side script)
	Application: SampleApplication
	Project: SampleProject
	Author: Luiz Cançado
	Description: A sample client-side script to demonstrate IDEAS application structure.
*/

var TAGS; // must be defined inside the screen script

var Ideas = {
    
    GotoURL: function (url) 
    {
        window.location(url);
    }   
}

function GotoScreen(scr) 
{
    window.location = '/?s=' + scr; 
}

// Function to handle SVG onLoad event
function init(evt) 
{
	if ( window.svgDocument == null )
	{
		svgDocument = evt.target.ownerDocument;
	}

	RegisterTags();

	InitScreen();
 
    UpdateVars();   
}

function UpdateVars() 
{
    ReadTagList();

    UpdateStats(); // must be defined inside the screen script
    
    setTimeout("UpdateVars()", 1000);
}

// Function to retrieve/write tag data from/to webservice
function ReadTag(tagName) 
{	
	bodyContent = $.ajax({
	    type: "GET",
	    url: "/TagsServer.asmx/Read?tagname=" + tagName,
	    contentType: "text/xml; charset=utf-8",
	    dataType: "xml",
        timeout: 1000,   
	    success: 
	    	function(xml) {	 
                var text = $(xml).find('string').text();
                var myArray = text.split('=');
	     		TAGS[myArray[0]] = myArray[1];
	    	},
        error: 
            function(msg) {
                //alert(msg);
            }      
	  }).responseText;
}

function ReadTagList()
{	
	for(key in TAGS)
	{
	   ReadTag(key);
	}
}

function WriteTag(tagName, tagValue) 
{   
    $.ajax({
        type: "GET",
        url: "/TagsServer.asmx/Write?tagname=" + tagName + "&value="+ tagValue,
        contentType: "text/xml; charset=utf-8",
        dataType: "xml",
        success: 
            function(xml) {  
                alert("Written successfully.");
            }
      });
}


// Animation functions

function SetVisibility(objID, value)
{
    obj = svgDocument.getElementById(objID);

    if(obj != null)
    {
        if(value == false)
        {
            //obj.setAttribute("visibility", "hidden");
            obj.setAttribute('style','display:none');
        }
        else
        {
            //obj.setAttribute("visibility", "visible");
            obj.setAttribute('style','display:inline');
        }
    }
    else
    {
        alert("Error: Object '" + objID + "' not found.");
    }
}

function SetText(objID, value)
{
    obj = svgDocument.getElementById(objID);

    if(obj != null)
    {
        obj.firstChild.data = value;
    }
    else
    {
        alert("Error: Object '" + objID + "' not found.");
    }
}

function GetText(objID)
{
    obj = svgDocument.getElementById(objID);

    if(obj != null)
    {
        return obj.firstChild.data;
    }
    else
    {
        alert("Error: Object '" + objID + "' not found.");
    }
}

function SetPosition(objID, valueX, valueY)
{
    obj = svgDocument.getElementById(objID);

    if(obj != null)
    {
        obj.setAttribute('transform','translate(' + valueX + ',' + valueY + ')');
    }
    else
    {
        alert("Error: Object '" + objID + "' not found.");
    }
}

function SetFillColor(objID, color)
{
    obj = svgDocument.getElementById(objID);

    if(obj != null)
    {
        obj.style.setProperty('fill',color);
    }
    else
    {
        alert("Error: Object '" + objID + "' not found.");
    }
}