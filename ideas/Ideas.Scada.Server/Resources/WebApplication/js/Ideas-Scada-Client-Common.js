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

	RegisterTags();

	initScreen();
}

function RegisterTags()
{
	TAGS = {
        AUTOMATICO: "",
        BICO_A: "",                             
        BICO_B: "",                             
        DESLIGA_ESTEIRA: "",                    
        EMERGENCIA: "",                         
        ESTEIRA_LIGADA: "",                     
        LIGA_ESTEIRA: "",                       
        MANUAL: "",                             
        MISTURADOR: "",                         
        PRODUTO_A: "",                          
        PRODUTO_B: "",                          
        PRODUTO_C: "",                          
        S1: "",                                
        S2: "",                                
        S3: "",                                
        S4: "",                                
        S5: "",                                
        T_MISTURADOR: "",                       
        T_PROD_A_BICO_A: "",                    
        T_PROD_A_BICO_B: "",                    
        T_PROD_B_BICO_A: "",                    
        T_PROD_B_BICO_B: "",                    
        T_PROD_C_BICO_A: "",                    
        T_PROD_C_BICO_B: ""                   	
	}
}


// Function to retrieve tag data from webservice
function ReadTag(tagName) 
{	
	$.ajax({
	    type: "GET",
	    url: "/TagsServer.asmx/GetTagValue?tagname=" + tagName,
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