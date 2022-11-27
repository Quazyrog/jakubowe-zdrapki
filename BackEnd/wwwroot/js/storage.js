

class DoneDaysStorage
{
    static _dateToString(date)
    {
        return date.toISOString().substring(0,10);
    }
    
    constructor(calendarName)
    {
        this._storageName = `DoneDays.${CALENDAR_NAME}`;
        this._cache = null;
    }
    
    _refreshCache()
    {
        const storageString = localStorage.getItem(this._storageName) || '[]';
        this._cache = new Set(JSON.parse(storageString));
    }
    
    _flushCache()
    {
        localStorage.setItem(this._storageName, JSON.stringify(Array.from(this._cache)));
    }

    isDayTaskDone(date)
    {
        const d = DoneDaysStorage._dateToString(date);
        if (this._cache === null) {
            this._refreshCache();
        }
        return this._cache.has(d);
    }
    
    markTaskNotDone(date)
    {
        this._refreshCache();
        const d = DoneDaysStorage._dateToString(date);
        this._cache.delete(d);
        this._flushCache();
    }

    markTaskDone(date)
    {
        this._refreshCache();
        const d = DoneDaysStorage._dateToString(date);
        this._cache.add(d);
        this._flushCache();
    }
}