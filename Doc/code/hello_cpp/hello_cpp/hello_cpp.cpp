// hello_cpp.cpp: 主项目文件。

#include "stdafx.h"

using namespace System;
using namespace System::ComponentModel;
using namespace System::Collections;
using namespace System::Windows::Forms;
using namespace System::Data;
using namespace System::Drawing;
using namespace AddIn::Core;

[STAThreadAttribute]
int main(array<System::String ^> ^args)
{
	AppFrame app;
	app.Run();
	return 0;
}
