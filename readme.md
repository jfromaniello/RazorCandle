RazorCandle
===========

Render razor template and save the content to a file.

Options
=======

	RazorCandle.exe source  [destination] [/M]  [/V]
	
	source         Specifies the source razor file.
	[destination]  Specify the output file. By default is the same name as the
	               source with the html extension.
	[/?]           Show Help
	[/M]           Json model as string to the model.
	[/V]           Verbose mode. Show result in the output.

Usage
=====

Executing this:

	RazorCandle.exe myfile.cshtml

will render the template with razor and create a new file myfile.html.

Models
------

Suppose you have a view as follow:

	<html>
		<head>
		<title>Hello</title>
		</head>
		<body>
			<p>hello @Model.FirstName @Model.LastName</p>
		</body>
	</html>
	
You can pass a model as JSON as follows
	
	RazorCandle.exe myfile.cshtml /M="{FirstName: 'Alberto', LastName: 'Perez' }"

and it will generate this html:

	<html>
		<head>
		<title>Hello</title>
		</head>
		<body>
			<p>hello Alberto Perez</p>
		</body>
	</html>

Subtemplates
------------

You can use RenderPartial as in Asp.Net MVC projects as follows:

	<html>
		<head>
		<title>Hello</title>
		</head>
		<body>
		@Html.Partial("..\\relative\\Nested.cshtml")
		</body>
	</html>

The path should be relative to the location of the calling template always. Nesting in N-level is okay.

Installing
==========

All you need is RazorCandle.exe, dependencies are embedded.
You can download it from here.
