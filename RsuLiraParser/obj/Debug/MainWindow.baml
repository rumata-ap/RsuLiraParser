   M S B A M L   `   `   `  ����H  DRsuLiraParser, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:RsuLiraParserRsuLiraParser  5   	       RsuLiraParser.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   -localclr-namespace:RsuLiraParser     5   	     � Title$;  5Преобразование РСУ Лира-САПР��5   	   $
��1080��5	   	   $
��1920��6    � SizeToContent$ WidthAndHeight=�6*    � 
ResizeMode$ NoResize=�5
   	    � WindowStartupLocation$ CenterScreen=�6    � DataContext 5        RsuLiraParser.ViewModel  5   
    � 	Resources 5         
ColorGroup  
ColorLabel��%V   &	        &	 !     ��5   
   	#FF022EF5��6.   ��5   
   	#FF17A608��6.   5      .��5      � -   5         �Name$ mainGrid��5      $	��400��6   .+�5   
   �� $��5q�6   .+�5      �� $;��5Выбор файлов исходных данных��6   $��14(�6=   ��5      �� 5       �� Color# �   6&   .��5      �� .+�5      �� $��12(�5       $
��Wrap=�5       ���	#FF17A608 �� TextAlignment$ Center=�5       ��5       �� *
PatchRsu+5       �� 5       ��6   �� *StatusReadRsu+��6@   �� *SelectRsuFile+�� 5!      $��12(�5"       $
��Wrap=�5#       ���	#FF17A608$ Center=�5%       ��5!       �� *	PatchRsuL+5%       �� 5&      ��6   �� *StatusReadRsuL+��6A   �� *SelectRsuLFile+�� 5'      $��12(�5(       $
��Wrap=�5)       ���	#FF17A608$ Center=�5+       ��5'       �� *	PatchRsuN+5+       �� 5,      ��6   �� *StatusReadRsuN+��6A   �� *SelectRsuNFile+�� 5-      $��12(�5.       $
��Wrap=�5/       ���	#FF17A608$ Center=�51       ��5-       �� *
PatchRsuNL+51       �� 52      ��6   �� *StatusReadRsuNL+��6B   �� *SelectRsuNLFile+53      54      �� 56      $1��+Сохранение результатов��6   $��14(�67   ��57      �� 58      # �   6&   .��5:      �� .+�5;      �� $��12(�5<       $
��Wrap=�5=       ���	#FF17A608$ Center=�5?       ��5;       �� *	PathXML+5?       �� 5@      ��6   �� *	StatusXML+��6<   �� *WriteRsuToXMLFile+�� 5A      $��0 15 0 0q�6   $1��+Создать базу данных SQLite��5B      ��5C      �� *CreateSQLiteDb+�� 5D      $��12(�5E       $
��Wrap=�5F       ���	#FF17A608$ Center=�5H       ��5D       �� *PathSQLiteDB+5H       �� 5I      -   5        	 ��Name$	 
SQLsaveBtn��5I      ��61   �� *	StatusSQL+��6P   �� *SaveSQLiteDb+5J      5K      �� 5L      -   5        $	 btCancel��5L      $��
Сброс��6'   $��14(�5N      ���	#FF9FD7FD$
��auto��5P      ��5M      �� *
UpdateFull+5Q      �� 5S      -   5        
 ��Name$
 	statusBar��5S      $��0 3 0 0q�6+   .@�5T      � .+�5U      � -   5         �Name$ pbStatus��5U   "    � Minimum$ 0Q�5V   "   $��15��5Y   "   $��Stretch=�5[   "    � Maximum 5W   "   �� *
CountRsu+ � Value 5X   "   �� *	Counter+��5Z      ��  �� ElementName$ mainGrid�� �� Path$ Width�5[   "   �� 5\      $��Center=�6>   ��6    �� *ProgressHint+6>   5]      5^      5_      5a      5b      