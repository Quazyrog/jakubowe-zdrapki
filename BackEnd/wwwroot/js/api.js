function _parseDateFields(fields, data)
{
    for (let field of fields) {
        data[field] = new Date(data[field]);
    }
    return data;
}

function getTodayTask() 
{
    const uri = '/api/tasks/today';
    return fetch(uri)
        .then(response => response.json())
        .then(data => _parseDateFields(['date'], data));
}

function getCalendar(name)
{
    const uri = '/api/calendars/' + name;
    return fetch(uri)
        .then(response => response.json())
        .then(data => data.map(day => _parseDateFields(['date'], day)));
}