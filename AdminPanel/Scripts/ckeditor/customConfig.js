CKEDITOR.editorConfig = function (config) {
	// Define changes to default configuration here.
	// For complete reference see:
	// http://docs.ckeditor.com/#!/api/CKEDITOR.config

	// The toolbar groups arrangement, optimized for a single toolbar row.
	config.toolbarGroups = [
		{ name: 'basicstyles', groups: ['basicstyles'] },
		{ name: 'paragraph', groups: ['align'] },
		{ name: 'styles' },
		{ name: 'colors' },
	];

	// The default plugins included in the basic setup define some buttons that
	// are not needed in a basic editor. They are removed here.
	config.removeButtons = 'Cut,Copy,Paste,Undo,Redo,Anchor,Underline,Strike,Subscript,Superscript,Save,Preview,Form,TextField,Subscript,Superscript,CreateDiv,Language,Anchor,Flash,Smiley,Iframe,PageBreak,Maximize,ShowBlocks,About,SpecialChar,Checkbox,Radio,Print,Button,Search';

	// Dialog windows are also simplified.
	config.removeDialogTabs = 'link:advanced';

	config.contentCss = ['/ckeditor/skins/moono-lisa/editor.css']
};
