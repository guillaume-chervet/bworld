
§if(model.ColorBackgroundMenu)§
.navbar-default {
	background: §model.ColorBackgroundMenu§;
}
.navbar-default .navbar-collapse .navbar-nav > .active > a, .navbar-default .navbar-collapse .navbar-nav > .active > a:hover, .navbar-default .navbar-collapse .navbar-nav > .active > a:focus {
	background-color: §model.ColorSelectedBackgroundMenu§;
}
.navbar-default .navbar-collapse .navbar-nav .open .dropdown-menu > .active > a, .navbar-default .navbar-collapse .navbar-nav .open .dropdown-menu > .active > a:hover, .navbar-default .navbar-collapse .navbar-nav .open .dropdown-menu > .active > a:focus {
	background-color: §model.ColorSelectedBackgroundMenu§;
}
.navbar-default .navbar-collapse>.open>a, .navbar-default .navbar-collapse .navbar-nav>.open>a:hover, .navbar-default .navbar-collapse .navbar-nav>.open>a:focus {
	background-color: §model.ColorHoverBackgroundMenu§;
}
nav>li>a:hover {
	background-color: §model.ColorHoverBackgroundMenu§;
}
.navbar-default .navbar-nav>li>a:hover {
	background-color: §model.ColorHoverBackgroundMenu§;
}

.mw-submenuitem .open a:hover {
	background-color: §model.ColorHoverBackgroundMenu§;
}

§endif§ 

§if(model.ColorBackgroundTableHeader)§
tfoot, thead {
    background-color: §model.ColorBackgroundTableHeader§;
    color: white;
}

.panel-default {
    border-color: §model.ColorBackgroundTableHeader§;
}
.panel-default>.panel-heading {
    color: #fff;
    background-color: §model.ColorBackgroundTableHeader§;
    border-color: §model.ColorBackgroundTableHeader§;
}
.panel-footer {
    background-color: §model.ColorBackgroundTableFooter§;
    border-top: 1px solid §model.ColorBackgroundTableFooter§;
	color: #fff;
}

.mw-news .panel-heading h3 a.mw-title, .mw-news .panel-heading h3 a.mw-title:link, .mw-news .panel-heading h3 a.mw-title:visited {
    color: #fff;
}

.mw-news .mw-author {
    color: white;
}

§endif§ 

§if(model.ColorSeparator)§
hr.mw-hr {
	background-color: §model.ColorSeparator§;
}
§endif§ 

§if(model.Color)§
body {
	color: §model.Color§;
}
§endif§ 

§if(model.ColorBackground)§
body {
	background-color: §model.ColorBackground§;
}
§endif§ 

§if(model.ColorH1)§
h1 {
	color: §model.ColorH1§;
}
§endif§ 
§if(model.ColorH2)§
	h2 {
		color: §model.ColorH2§;
	}

	legend {
	    color: §model.ColorH2§;
		border: 0px;
	}

	.mw-btn-link {
		background-color: §model.ColorH2§;
		border-color: §model.ColorH2§;
	}

§endif§
§if(model.ColorH3)§
h3 {
	color: §model.ColorH3§;
}
§endif§ 
§if(model.ColorJumbotron)§
	.mw-jumbotron {
		background-color: §model.ColorJumbotron§;
	}
§endif§

§if(model.ColorBackgroundMenuBottom)§
	footer {
		background-color: §model.ColorBackgroundMenuBottom§;
		color:white;
	}
	footer .mw-footer-links {
		color:white;
	}

	footer a:visited {
		color: #a7adff;
	}
	footer a:link {
		color: #a7adff;
	}
	footer a {
		color: #a7adff;
	}

§endif§

.circularG{
	background-color: §model.ColorLoader§;
}