/*
	Ideas.Scada.Server - 2011
	
	Screen: Diagnostico
	File: Diagnostico.svg.js (Client-side script)
	Application: SampleApplication
	Project: SampleProject
	Author: Luiz Cançado
	Description: A sample client-side script to demonstrate IDEAS application structure.
*/

var click = 0;
var mouseDown = 0; 
var mouseUp = 0;
var mouseOver = 0;
var mouseMove = 0;
var mouseOut = 0;
var textbutton1;

 
function RegisterTags()
{
	TAGS = {
        AUTOMATICO: "",         
		BICO_A: "", 
		BICO_B: "", 
		BICO_C: "", 
		//CONT_PROD_A: "",  
		//CONT_PROD_B: "",  
		//CONT_PROD_C: "",  
		//DESLIGA_ESTEIRA: "", 
		//EMERGENCIA: "",  
		//ESTEIRA: "",   
		//LIGA_ESTEIRA: "",    
		//MANUAL: "",  
		//MISTURADOR: "",  
		//PRODUTO_A: "",       
		//PRODUTO_B: "",       
		//PRODUTO_C: "",       
		//RESET: "", 
		//S1: "", 
		//S2: "", 
		//S3: "", 
		//S4: "", 
		//S5: "", 
		//S6: "", 
		//T_MISTURADOR: "",   
		//T_PROD_A_BICO_A: "",
		//T_PROD_A_BICO_B: "",
		//T_PROD_A_BICO_C: "",
		//T_PROD_B_BICO_A: "",
		//T_PROD_B_BICO_B: "",
		//T_PROD_B_BICO_C: "",
		//T_PROD_C_BICO_A: "",
		//T_PROD_C_BICO_B: "",
		//T_PROD_C_BICO_C: ""                	
	} 
}


//now create a new button instance
function InitScreen(evt)
{
/*
	textbutton1 = 
		new button(
			"Botao1",
			"Botao",
			GotoScreen,
			"rect",
			"Click me",
			undefined,
			500,
			300,
			100,
			30,
			{"font-family":"Arial,Helvetica","fill":"navy","font-size":12},
			{"fill":"lightsteelblue"},
			{"fill":"white"},
			{"fill":"navy"},
			1);
*/
}

   
function UpdateStats()
{
	for(key in TAGS)
	{
		var object = svgDocument.getElementById(key + "_value");
		if(object != null)
		{
			object.firstChild.data = TAGS[key];
		}
	}
}


// Event Circle - Functions -- START

function updateCounters() 
{
	svgDocument.getElementById("clicks").firstChild.data = "onclick = " + click;
	svgDocument.getElementById("mousedowns").firstChild.data = "onmousedown = " + mouseDown;
	svgDocument.getElementById("mouseups").firstChild.data = "onmouseup = " + mouseUp;
	svgDocument.getElementById("mouseovers").firstChild.data = "onmouseover = " + mouseOver;
	svgDocument.getElementById("mousemoves").firstChild.data = "onmousemove = " + mouseMove;
	svgDocument.getElementById("mouseouts").firstChild.data = "onmouseout = " + mouseOut;
}
    
function msClick (evt) {
	click++;
	updateCounters();
}
 
function msDown (evt) {
	mouseDown++;
	updateCounters();
}

function msUp (evt) {
	mouseUp++;
	updateCounters();
}

function msOver (evt) {
	mouseOver++;
	updateCounters();
}

function msMove (evt) {
	mouseMove++;
	updateCounters();
}

function msOut (evt) {
	mouseOut++;
	updateCounters();
}

// Event Circle - Functions -- END