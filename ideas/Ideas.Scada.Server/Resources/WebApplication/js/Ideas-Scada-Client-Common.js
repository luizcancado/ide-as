/*
	Ideas.Scada.Server - 2011
	
	Screen: tela
	File: tela.svg.js (Client-side script)
	Application: SampleApplication
	Project: SampleProject
	Author: Luiz Cançado
	Description: A sample client-side script to demonstrate IDEAS application structure.
*/


var Ideas = {
    
    GetTagValue: function (tagName) 
    {
        return "";
    }
    
}

function init(evt) 
{
	if ( window.svgDocument == null )
	{
		svgDocument = evt.target.ownerDocument;
	}
	 alert('teste') ;
	updateVars();
}

var TAGS = [];
 
function updateVars() 
{
	for(i = 0; i < 20; i++)
	{
		TAGS[i] = Math.floor(Math.random()*101);
		//TAGS[i] = callWebService();
	}

	updateStats();
	
	setTimeout ( "updateVars()", 1000 );
}
    
function updateStats()
{
	for(i = 0; i < 20; i++)
	{
		var object = svgDocument.getElementById("Label" + (i + 1));
		if(object != null)
		{
			object.firstChild.data = TAGS[i];
		}
	}
}
  
                   
// Web Service functionality

// Calls web service with url and callback function. Callback will
// be executed when XMLHttpRequest object returns from web service call.
function callWebService(url, callback)
{
    var xmlDoc = null;
    
    if (window.XMLHttpRequest) 
    {
        xmlDoc = new XMLHttpRequest(); //Newer browsers
       
    }

    if (xmlDoc) 
    {
        //callback for readystate
        xmlDoc.onreadystatechange = function() { stateChange(xmlDoc, callback); };
        xmlDoc.open("GET", url, true); //set for async "get"
        xmlDoc.send(null); //execute asynchronous call to web service
    }
    else 
    {
        if(callback)   
            callback(false, "unable to create XMLHttpRequest object");
    }
}


// Updates readystate by callback
function stateChange(xmlDoc, callback)
{
    if (xmlDoc.readyState == 4) 
    {
        var text = "";

        if (xmlDoc.status == 200) 
        {
            //select node containing data
            var nd = xmlDoc.responseXML.documentElement.getElementsByTagName("date_time");
           
            if (nd && nd.length == 1)
            { //IE use .text, others .textContent
                text = nd[0].textContent;
            }
        }

        // Perform callback with data if callback function signature was provided, 
        // alternately we could perform UI update right here vs a callback.
        if(callback != null)
            callback(text != "", text);
	}
}

// Callback pointer supplied to callWebService function to
// display results of web service call
function callbackTest(result, data)
{
    if (result)
        alert("Success " + data);
    else
        alert("Web service call failed " + data);
}     

        
            
                
                    
                       




