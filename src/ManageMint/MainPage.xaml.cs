using ManageMint.Pages;

namespace ManageMint;

public partial class MainPage : ContentPage
{
	public MainPage(Diary diary)
	{
		InitializeComponent();
		
		BindingContext = diary;
	}
}

