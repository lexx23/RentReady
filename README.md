<b>Эндпоинты приложения:</b><br>
		GET -> https://xxx.azurewebsites.net/api/timeentity<br>
		POST-> https://xxx.azurewebsites.net/api/timeentity<br>

Request body:
{
    "startOn": "2021-12-20 00:00:00",
    "endOn": "2021-12-26 05:00:00"
}




<b>Создание приложения, портал Azure:</b>
1. Переходим на портал Azure https://portal.azure.com/#home
2. Создаем ресурс Azure Function 
<img src="https://user-images.githubusercontent.com/1942344/148208025-32bab7c8-13e4-4d44-997e-e07d7704f961.png">  
3. Указать имя приложения. Это имя понадобится позже, при деплое.
<img src="https://user-images.githubusercontent.com/1942344/148208359-e3e90b63-fe57-4722-a5b0-6b1c4d3d294d.png">  


<b>Azure DevOps</b>
1. Переходим на Azure DevOps https://dev.azure.com/
2. Создаем новый проект.
3. Необходимо добавить новое соединение. Переходим в Project Settings-> Service Connections
<img src="https://user-images.githubusercontent.com/1942344/148215888-7ebf7728-07ef-42ec-ad16-867c169543f4.png">
<img src="https://user-images.githubusercontent.com/1942344/148216139-1fb5bdfa-2cad-4ed1-a302-dbe13f5c3f3c.png">
<img src="https://user-images.githubusercontent.com/1942344/148216233-61342069-7db6-4f20-84e5-7873eed387dc.png">

4. Добавляем новый Pipeline 
<img src="https://user-images.githubusercontent.com/1942344/148203324-1f85e733-3991-4cf9-bcb4-5c904a0a2055.png">  
5. Выбираем Github
<img src="https://user-images.githubusercontent.com/1942344/148203539-cdf95f45-cd77-4f91-9f3c-6083f52af7d6.png"> 
6. Выбираем, из доступных проектов, <b>RentReady</b>
<img src="https://user-images.githubusercontent.com/1942344/148205588-e3f73d19-5a98-4a0a-929b-a6e8ebec3801.png"> 
7. Необходимо добавить переменные: Dataverse_UserName, Dataverse_Password, Dataverse_Environment, functionName<br>
<img src="https://user-images.githubusercontent.com/1942344/148214703-b96a0fac-d237-4d19-9c93-9f21d0cb5833.png"> 
	- В них указать, параметры от Power App, а так же, название приложения.<br>
		-	Dataverse_UserName - имя пользователя<br>
		-	Dataverse_Password - пароль<br>
		-	Dataverse_Environment - часть URL. Пример: org9b44749a.crm4 <br>
		- functionName - имя приложения. Использовать значение, полученное при создании приложения (шаг 3)<br>
8. На Deploy шаге, возможно, потребуется подтвердить права. Дать разрешение. 
<img src="https://user-images.githubusercontent.com/1942344/148217046-421b9c70-9110-45bc-bf3d-085f9cc58f73.png"> 

