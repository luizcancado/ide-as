/*
	Ideas.Scada.Server - 2011
	
	Screen: tela
	File: tela.svg.js (Client-side script)
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
