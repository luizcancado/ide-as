/*
	Ideas.Scada.Server - 2011
	
	Screen: tela
	File: tela.svg.js (Client-side script)
	Application: SampleApplication
	Project: SampleProject
	Author: Luiz Cançado
	Description: A sample client-side script to demonstrate IDEAS application structure.
*/

var objTambor;

function RegisterTags()
{
	TAGS = {
        AUTOMATICO: "", // TODO          
		BICO_A: "",
		BICO_B: "",
		BICO_C: "",
		CONT_PROD_A: "", // TODO
		CONT_PROD_B: "", // TODO    
		CONT_PROD_C: "", // TODO    
		DESLIGA_ESTEIRA: "", 
		EMERGENCIA: "", 
		ESTEIRA: "", // TODO    
		LIGA_ESTEIRA: "",   
		MANUAL: "", // TODO
		MISTURADOR: "", // TODO 
		PRODUTO_A: "",      
		PRODUTO_B: "",      
		PRODUTO_C: "",      
		RESET: "", // TODO
		S1: "", 
		S2: "", 
		S3: "", 
		S4: "", 
		S5: "", 
		S6: "", 
		T_MISTURADOR: "",   
		T_PROD_A_BICO_A: "",
		T_PROD_A_BICO_B: "",
		T_PROD_A_BICO_C: "",
		T_PROD_B_BICO_A: "",
		T_PROD_B_BICO_B: "",
		T_PROD_B_BICO_C: "",
		T_PROD_C_BICO_A: "",
		T_PROD_C_BICO_B: "",
		T_PROD_C_BICO_C: ""                	
	} 
}

function InitScreen(evt)
{
	
}

function UpdateStats()
{
	UpdateEmergencia();
	UpdateSensores();
	UpdateBicosTinta();
	UpdateMisturador();
}

function UpdateMisturador()
{ 
	if(TAGS["MISTURADOR"] == 1)
	{
		SetFillColor("IndicadorMisturadorLigado", "#00AA00");
		SetFillColor("IndicadorMisturadorDesligado", "#666666");
		SetPosition("Misturador", 755, 300);
	}
	else
	{
		SetFillColor("IndicadorMisturadorLigado", "#666666");	
		SetFillColor("IndicadorMisturadorDesligado", "#AA0000"); 
		SetPosition("Misturador", 755, 400);
	}
}

function UpdateBicosTinta()
{
	if(TAGS["BICO_A"] == 1)
	{
		SetVisibility("TintaAmarela", true);
	}
	else
	{ 
		SetVisibility("TintaAmarela", false);
	}
	
	if(TAGS["BICO_B"] == 1)
	{
		SetVisibility("TintaVermelha", true);
	}
	else
	{ 
		SetVisibility("TintaVermelha", false);
	}
	
	if(TAGS["BICO_C"] == 1)
	{
		SetVisibility("TintaAzul", true);
	}
	else
	{ 
		SetVisibility("TintaAzul", false);
	}
}

function UpdateEmergencia()
{
	if(TAGS["EMERGENCIA"] == 1)
	{
		SetVisibility("Botao_emergencia", true);
		SetText("btnTextoEmergencia", "Emergencia");
	}
	else
	{
		SetVisibility("Botao_emergencia", false);
		SetText("btnTextoEmergencia", "");
	}
}

function UpdateSensores()
{
	if(TAGS["S1"] == 1)
	{
		SetPosition("Tambor", 235, 0);
		SetFillColor("LabelS1", "#ff0000");
	}
	else 
		SetFillColor("LabelS1", "#000000");
	
	if(TAGS["S2"] == 1)
	{
		SetPosition("Tambor", 350, 0);
		SetFillColor("LabelS2", "#ff0000");
	}
	else 
		SetFillColor("LabelS2", "#000000");
		
	if(TAGS["S3"] == 1)
	{
		SetPosition("Tambor", 470, 0);
		SetFillColor("LabelS3", "#ff0000");
	}
	else 
		SetFillColor("LabelS3", "#000000");
		
	if(TAGS["S4"] == 1)
	{
		SetPosition("Tambor", 589, 0);
		SetFillColor("LabelS4", "#ff0000");
	}
	else 
		SetFillColor("LabelS4", "#000000");
		
	if(TAGS["S5"] == 1)
	{
		SetPosition("Tambor", 755, 0);
		SetFillColor("LabelS5", "#ff0000");
	}
	else
		SetFillColor("LabelS5", "#000000");
}