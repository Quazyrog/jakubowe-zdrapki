function getTodayTask() {
    const uri = 'api/tasks/today';
    
    fetch(uri)
        .then(response => response.json())
        .then(data => _displayToday(data))
        .catch(error => console.error('Unable to get items.', error));
}

function _displayToday(data)
{
    const element = document.getElementById("today_task");
    element.textContent = data.text;
}