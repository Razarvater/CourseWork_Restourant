   M S B A M L   `   `   `  ���� L  HWindowControllers, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:WindowControllersWindowControllers  C&clr-namespace:WindowControllers.ThemesWindowControllers.Themes  &     WindowControllers.Themes.GenericR NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089  �� TypeInTargetAssembly&    WindowControllers.Themes.Generic �� 
ResourceId*  $WindowControllers.ButtonVisConverter   ButtonVisConverter(  "WindowControllers.MenuVisConverter  MenuVisConverter)  #WindowControllers.MenuModeConverter  MenuModeConverter*  $WindowControllers.MenuColorConverter  MenuColorConverter  BorderGapMaskConverter �� 
TargetType �� Property �� Value  ScrollBarLineButtonStyle �� 
TargetType � Points " ScrollBarLineButtonBottomStyle  ScrollBarPageButtonStyle  ScrollBarThumbStyle 	 ScroolBarBlackVertical � RowDefinitions ]� DecreaseRepeatButton	 ]� Thumb
 ]� IncreaseRepeatButton 
 ScrollBarLineButtonStyleHor % !ScrollBarLineButtonBottomStyleHor  ScrollBarThumbStyleHor  ScroolBarBlackHorizontal � ColumnDefinitions �� CanContentScroll ��Name �� VerticalOffset � Value �� ScrollableHeight � Maximum �� ViewportHeight �� ViewportSize* �� #ComputedVerticalScrollBarVisibility �� Orientation �� HorizontalOffset �� ScrollableWidth �� ViewportWidth, �� %ComputedHorizontalScrollBarVisibility d�Name �� SelectionBoxItem d� 	IsChecked �� Mode �� RelativeSource �Name  � AllowsTransparency! ��Name" ��Name# ��Name$ K� IsItemsHost-  'System.Windows.Input.KeyboardNavigation%   DirectionalNavigation& �� CornerRadius' J� SnapsToDevicePixels( J� Effect3  -System.Windows.Media.Effects.DropShadowEffect)   
BlurRadius*   Color+   Opacity,   ShadowDepth- �� Triggers. P� Property/ P� Value0 �� 
TargetName1 �� Triggers2 �� Path3 N� EnterActions4 �� TargetProperty5 g� Duration6 �� From	7 �� To8 g� AutoReverse9 g� RepeatBehavior: ��Name; ��Name	< �� X1	= �� X2	> �� Y1	? �� Y2@ ��NameA �� PaddingB �� BasedOnC �� BindingD �� Value	E W� ToF W� EasingFunction.  (System.Windows.Media.Animation.CubicEaseG N� ExitActions  ManageButtonH �� ScaleXI �� ScaleYJ ��Name  	PageItemsK   RenderingBiasL   	DirectionM �� 	ConverterN �� Source  MainMenuO $� Orientation0	   *WindowControllers.RestourantWindowTemplateP �� UpdateSourceTriggerQ ]�NameR �� OrientationS �� EdgeModeT � PointsU K� ZIndexV �� BottomW �� LeftX �� AncestorTypeY � IconZ � Title[ �� TextAlignment\ �� ImageSource] j� Stretch^ %� IsVirtualizing'
  !System.Windows.Shell.WindowChrome_ 
  NonClientFrameEdges` 
  CornerRadiusa 
  ResizeBorderThicknessb 
  CaptionHeight   D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    :x,http://schemas.microsoft.com/winfx/2006/xaml     )localclr-namespace:WindowControllers  -lc&clr-namespace:WindowControllers.Themes  %�6  (��       "   $ MenuBack��)(��      "   $ MenuBackFocused��)(�� &     "   $ MenuBackNotFocused��)(�� 9     "   $ ButtonFocused��)(�� L     "   $ ButtonNotFocused��)(�� _     "   $ ButtonDisabled��)(�� r     "   $ ScrollBarColor��)(�� �     "   $ GroupBoxBackColor��)(�� �     "   $ GroupBoxBackColor_2��)(�� �     "   $ GroupBoxBorderColor��)(�� �     "   $ FocusedTextBoxBackground��)(�� �     "   $! NotFocusedTextBoxBackground��)(�� �     "   $ DisabledTextBoxBackground��)(�� �     "   $ MainFontColor��)(�� 
    "   $ FontColorNotFocused��)(��     "   $ FontColorFocused��)(�� 0    "   $ MenuFontColorNotFocused��)(�� C    "   $ MenuFontColorFocused��)(�� V    "   $ MenuFontColorPressed��)(�� i    "   $ MenuFontColorActive��)&	  |    &	 �    &	 �    &	 �    &	 �    (�� �    "   $ 	ListBlack��)&	 �    0�� *�� $  
lc:GenericL�$ ScrollBarColor��+1&	 k    0�� *�� $  
lc:GenericL�$ ScrollBarColor��+1&	 �    0�� *�� $  
lc:GenericL�$ ScrollBarColor��+1&	 N    0�� *�� $  
lc:GenericL�$ ScrollBarColor��+1&		 �    7  7  7  7  7  &	
 7    0�� *�� $  
lc:GenericL�$ ScrollBarColor��+1&	 �    0�� *�� $  
lc:GenericL�$ ScrollBarColor��+1&	 ?    0�� *�� $  
lc:GenericL�$ ScrollBarColor��+1&	 �    7 
 7  7  7  7  (�� -    "   $ ScrollViewerBlack��)7 	 7  (�� g	    "   $ ComboBoxBlack��)0�� *�� $  
lc:GenericL�$! NotFocusedTextBoxBackground��+10�� *�� $  
lc:GenericL�$ MainFontColor��+1(�� �    "   $ TextBlockBlack��)0�� *�� $  
lc:GenericL�$ MainFontColor��+1(�� �    "   $ TextBoxBlack��)0�� *�� $  
lc:GenericL�$! NotFocusedTextBoxBackground��+10�� *�� $  
lc:GenericL�$ MainFontColor��+10�� *�� $  
lc:GenericL�$ ScrollViewerBlack��+10�� *�� $  
lc:GenericL�$ FocusedTextBoxBackground��+10�� *�� $  
lc:GenericL�$ DisabledTextBoxBackground��+1(�� .    "   $ CheckBoxBlack��)0�� *�� $  
lc:GenericL�$! NotFocusedTextBoxBackground��+10�� *�� $  
lc:GenericL�$ DisabledTextBoxBackground��+1(�� \    "   $ BlackButton��)0�� *�� $  
lc:GenericL�$ ButtonNotFocused��+10�� *�� $  
lc:GenericL�$ FontColorNotFocused��+10�� *�� $  
lc:GenericL�$ FontColorNotFocused��+10�� *�� $  
lc:GenericL�$ ButtonFocused��+10�� *�� $  
lc:GenericL�$ FontColorFocused��+10�� *�� $  
lc:GenericL�$ ButtonNotFocused��+10�� *�� $  
lc:GenericL�$ FontColorNotFocused��+10�� *�� $  
lc:GenericL�$ ButtonFocused��+10�� *�� $  
lc:GenericL�$ ButtonDisabled��+1(�� `    "   $ GroupBoxBlack��)0�� *�� $  
lc:GenericL�$ GroupBoxBackColor��+10�� *�� $  
lc:GenericL�$ GroupBoxBorderColor��+10�� *�� $  
lc:GenericL�$ MainFontColor��+1(��     "   $ GroupBoxDropMenu��)0�� *�� $  
lc:GenericL�$ GroupBoxBlack��+1&	 �    &	 �    0�� *�� $  
lc:GenericL�$ MenuBackNotFocused��+10�� *�� $  
lc:GenericL�$ MenuBackFocused��+17  0�� *�� $  
lc:GenericL�$ MenuFontColorActive��+10�� *�� $  
lc:GenericL�$ MenuFontColorFocused��+10�� *�� $  
lc:GenericL�$ MenuFontColorNotFocused��+10�� *�� $  
lc:GenericL�$ MenuFontColorPressed��+10�� *�� $  
lc:GenericL�$ MenuFontColorActive��+10�� *�� $  
lc:GenericL�$ MainFontColor��+1&	 f    7  0�� *�� $  
lc:GenericL�$ MenuBackNotFocused��+17  '	  �    0�� *�� $  
lc:GenericL�$ MenuBackNotFocused��+10�� *�� $  
lc:GenericL�$ MenuBackNotFocused��+17  7  7   7   7   7   7  0�� *�� $  
lc:GenericL�$ MainFontColor��+17  7  0�� *�� $  
lc:GenericL�$ MenuBackNotFocused��+1��	#FF3A3E52����	#FF515151����	#FF3A3E46����	#FF3A3E5B����	#FF3A3E52����	#FF3A3E43����	#FF626262����	#FF3A3E46����	#FF373b43����	#FF3A3E41����	#FF484C56����	#FF3E4248����	#FF3A3E43����	#FFCCCCCC����	#FFABABAB����	#FFACACAC����	#FFABABAB����	#FF447BD4����	#FF284A7E����	#FF66A3D2��        �� �� " ��.���  � ��
 ���� ��  � ��
 ���� ��  � ��$ 1q��� " ��.���  � |� .  ��  � �� �� " ��.R��� $��18��.+�� ' �   �� ���  $� ��� @T� �� ��2  �� " ��.���  � |� .  ��  � �� �� " ��.R��� $��18��.+�� ' �   �� ��-  $� ��- @T�  $� ��2  �� " ��.���  � �� .  ��  � |� .  ��  � �� �� " ��.R��� ��2  �� " n�.���  � �� .  ��  � |� .  ��  � ��$ 1,0,1,0q���  � �� �� " n�.R��� $��2,0,1,0q���2  �� " ��.���  � �� �� " ��.R��  �� $��18���� $��* ��� $��18��.+��� $��0��$��18��8��  $��ScrollBar.LineUpCommand��]� $��
PART_Track��$��1����.  �� $��ScrollBar.PageUpCommand��8�� 	 n� 8�� 
 �� $��ScrollBar.PageDownCommand��8�� �� $��3��$��18��8�� $��ScrollBar.LineDownCommand���� " ��.���  � |� .  ��  � �� �� " ��.R��� $��18��.+�� ' �   ��  z  $�  $�  $� �� ��2  �� " ��.���  � |� .  ��  � �� �� " ��.R��� $��18��.+�� ' �   ��  $� �� ��  $� @T� ��2  �� " n�.���  � �� .  ��  � |� .  ��  � ��$ 1,0,1,0q���  � �� �� " n�.R��� $��0,2,0,1q���2  �� " ��.���  � �� �� " ��.R��  �� $��18���� $��0.0001* ��� $��18��.+��� $��0��$��18��8��  $��ScrollBar.LineLeftCommand��]� $��
PART_Track��$��1�� �� $��ScrollBar.PageLeftCommand��8�� 	 n� 8�� 
 �� $ ��ScrollBar.PageRightCommand��8�� �� $��2��$��18��8�� $ ��ScrollBar.LineRightCommand���� " ��.���  � �� �� " ��.R��  �� �� $
��Auto � �� �� $
��Auto �.+��� $��0��$��0q�._��� # z���� $ PART_VerticalScrollBar��8��  $��1��# z # z # z #y�z �� $ PART_HorizontalScrollBar��8�� $ 
Horizontal=�$��1��$��1��# z # z # z #y�z �� " ��.���  � �� 2  ��  � �� 2 ��  � ��$ 12(���  � ��$ 0q���  � ��$ 30����  � ��$ 30����  � ��$ 1q���  � �� �� " ��.R�� .+�� #��z��#��z��#��z��.+�d� $ ToggleButton��#��z��#��z $��0q�#��z�� �� *IsDropDownOpen+$ TwoWay=� �� *TemplatedParent+� $ Popup��  . |�.  $��Bottom=�$��Slide=����� *IsDropDownOpen+ �� *TemplatedParent+.��� .+��� $! PopupBorder��#��z��._��� $" ScrollViewer��$
��Auto=�$
��Auto=�.���� $# ItemsPresenter��$ . $% 	Contained=��� " ��.���  � �� 2  ��  � ��$ 12(��� " ��.���  � ��$ 18(���  � �� 2  ��  � �� 2 ��  � ��
 ���� ��  � ��$ 3q���  � �� �� " ��.R�� .+��� $! border��$& 2��#��z��#��z��#��z��' . (   $) 11Q�$* Black��$	+ 0.5Q�$, 1Q��� $" PART_ContentHost��#{�z{�#��z��|�.  $��Hidden=�$
��Auto=���2 - P� . �@J�	IsFocused/ . . ���  2  � ���� $0 border�� �@J�Effect   $) 15Q�$* Black��$	+ 0.5Q�$, 1Q�P� . � {�/ .  . ���  2  � ��1 P� . �@=�HasError/ . . ���  �@�ToolTip ��  �� *Self+$)2 #(Validation.Errors)[0].ErrorContent���  � �� �	#55FF00003 �� .���� .���� $24 ,(Border.BorderBrush).(SolidColorBrush.Color)�$5 0:0:1D�$6 	#55AA0000X�$7 	#FFFF0000X�8 . $9 Forever���� " ��.���  � ��$ 15����  � ��$ 15����  � �� 2  ��  � �� �� " ��.R��� $! Border��$��1q�#��z��#��z��#{�z{�#��z��#��z��' . ._��� $: Value��$��15��$��15��$��Center=�$��Center=�.+��� $	; No1��$< 4��$= 11��$> 4��$? 11�����#FF0000$	��1.5���� $	; No2��$< 11��$= 4��$> 4��$? 11�����#FF0000$	��1.5���� $
; Yes1��$< 4��$= 7��$> 7��$? 11�����#00FF00$	��1.5���� $
; Yes2��$< 7��$= 11��$> 11��$? 4�����#00FF00$	��1.5��- P� . �@d�	IsChecked$
/ TrueY�. ��� $
0 Yes1�� � y�$ Visible=��� $
0 Yes2�� � y�$ Visible=��� $	0 No1�� � y�$ Hidden=��� $	0 No2�� � y�$ Hidden=�P� . �@d�	IsChecked$/ FalseY�. ��� $	0 No1�� � y�$ Visible=��� $	0 No2�� � y�$ Visible=��� $
0 Yes1�� � y�$ Hidden=��� $
0 Yes2�� � y�$ Hidden=�P� . � ��/ . . ��� $0 Border�� � �� �#d9d9d9�� $0 Border�� � ��$	 1.5q�P� . � {�/ .  . ��� $0 Border�� 2  � ���� " ��.���  � ��$ 11(���  � �� �� " ��.R�� .+��� $
! Back��$& 5��$	��0.1q���2  (   $) 10Q�$* Black��$
+ 0.35Q�$, 1Q��� $	@ txt��$��Center=�$��Center=�#��z��' . #��z��#��z����2 �� $@ txt_2��$��Center=�$y�Hidden=�$��Center=�#��z��' . #��z����2 - P� . �@J�IsMouseOver/ . . ��� $
0 Back�� 2  � ���� $	0 txt�� 2  � ��P� . �@J�IsMouseOver/ .  . ��� $
0 Back�� 2  � ���� $	0 txt�� 2  � ��P� . � ��/ . . ��� $
0 Back�� 2  � ���� $
0 Back�� �@J�Effect   $) 8Q�$* Black��$
+ 0.35Q�$, 1Q��� $
0 Back�� � ��$ 1q��� $	0 txt�� � ��$
 10.5(�P� . � {�/ .  . ��� $
0 Back�� 2  � ���� " ��.���  � ��$ 1q���  � �� 2  ��  � �� 2 ��  � �� 2 ��  � ��$ 16(���  � �� �� " ��.R�� ' .  �� $��1* � �� $
��Auto ��� $��1* �.+��� #��z��#��z��#��z��$& 4��$��2��(   $) 13Q�$* Black��$	+ 0.5Q�$, 0Q��� $! Header��$A 2,1,2,0q�$��Center=�._��� $��Header����. #' z' � $��1��.+��� #��z��#' z' �� " ��B 2  .���  � ��$ 0��1 �� C �� *IsMenuOpened+D True3 �� .���� .��W� $4 Height�$	E 150X�$5 0:0:1.4D�F   G �� .���� .��W� $4 Height�$E 0X�$5 0:0:1.4D�F   �� " ��.���  � ��
 ���� ��  � ��$ 0q���  � ��$ 	10,5,10,5q���  � ��$ 35����  � z� �� $H 1Q�$I 1Q���  � �� �� " ��.R�� .+��� $
! Back��$A 5q�
������ $& 0��$��0q�._��� $	J txt��$��Center=�$��Center=�#��z��- P� . �@J�IsMouseOver/ . . ��� $
0 Back�� C� *�� "   $ MenuBack��+ � ��P� . �@J�IsMouseOver/ .  . ��� $
0 Back�� � ��
 ���� P� . � ��/ . . ��� $
0 Back�� � ��
 ���� 1 P� . �@J�IsMouseOver/ . 3 �� .���� .��W� $4 (Button.Width)�$E 40X�$5 0:0:0.2D�G �� .���� .��W� $4 (Button.Width)�$E 35X�$5 0:0:0.2D��� " m�.���  � �� �� *Name+��  �@m�Command �� *
Navigate+��  �@m�CommandParameter �� *PageContent+��  �@m�Icon �� *
IconSource+��  � ��$ 12(���  � �� �� " m�.R��� $! Border��$��0q���2  ��2 ._��  �� $
��Auto �$��30���� $	��100 ��� $��5 � �� $��5 ��� $
��Auto ��� $��5 �.+��� $��1��$��30������ *
IconSource+�� $��1��$��1��$��6,3,6,5q�$��Center=�$��Header����. (   $
+ 0.25Q�$K Performance=�$	L 320Q��� $��2��$��1��$��Center=�$��Right=�$��4��$��10��y��� *
IsSelected+8M  .+��  �   @KL  	=  	= ��� ��2 - P� . �@J�IsMouseOver/ . . ���  �� N 2  � ��P� . �@J�IsMouseOver/ .  . ���  �� N 2  � ��P� . �@m�	IsPressed/ . . ���  �� N 2  � ���� $
D True��C �� $2 
IsSelected�.a���  �� N 2  � ����  � �� 2 �� " o�.���  � ��8   ��  � �� 2 ��  � �� �� .R�$� $��Center=�O �� *SelectedMenu+8M  �� " 	 .���  �@�AllowsTransparency . ��  �@�WindowStyle$
 None=���  �@�WindowState �� *WindowStateM+$P PropertyChanged=���  �@�
ResizeMode �� *	ResMode+$P PropertyChanged=���  �@�Left �� *Lft+$P PropertyChanged=�$ TwoWay=���  �@�Top �� *Tp+$P PropertyChanged=�$ TwoWay=���  � �� �� *Width+$P PropertyChanged=�$ TwoWay=���  � �� �� *Height+$P PropertyChanged=�$ TwoWay=���  � �� �� " �.R��� #��z��#��z��#��z��#��z����2  ._��  �� $
��Auto ��� $��* �.+�]� $Q 
BackBorder��$��0����2 .+��� $��Right=�$R 
Horizontal=�$S Aliased=�.+��� 8�� ���� *MinWindowCommand+.���� $��14��$��18��.+�� T �   �� ���  � ��� 
�������$��1���� 8�� ���� *MaxWindowCommand+.���� $��18��$��18��.+��� $U 1��$��12��$��12��
�������$V 2��$W 2���� $< 6��$> 4��$= 6��$? 2��
�������$��1��y��� *
RealMode+8M  �� $	< 5.5��$> 2��$= 16��$? 2��
�������$��1��y��� *
RealMode+8M  �� $< 16��$> 2��$= 16��$? 14��
�������$��1��y��� *
RealMode+8M  �� $< 16��$> 14��$= 14��$? 14��
�������$��1��y��� *
RealMode+8M  �� 8�� ���� *CloseWindowCommand+����  �� $X WindowL�.���� $��18��$��18��.+�� 
���  ��T �   ��- ��- ��� ��� � 
���  ��T �   ��� ��- ��- ��� ]� ���� " ]�.�-   .+��� #��zY $
��Left=�$��18��$��18��$��10,0,0,0q��� #��zZ $��10,0,0,0q�$
[ Left=�$��Center=���2	 ]� $��1��.+�o� 8��
 $U 1������ *SelectedMenu+y��� *
IsMenuOn+8M  ���� *
TabsList+�� $& 5��$��1q���2 ���� $&\  /Resources/Images/Background.png��$
] Fill=�._��� ^ . ���� *SelectedPage+- P� . �@�
ResizeMode$/ NoResize=�. ���  �@
 WindowChrome 
  $
_ None=�$` 0��$a 0q�$b 0Q�P� . �@�
ResizeMode$/ 	CanResize=�. ���  �@
 WindowChrome 
  $
_ None=�$` 0��$a 2q�$b 0Q�