namespace Akade.Workshops.Performance.Api.Data;

public enum ElementType
{
    /// <summary>
    /// Precipitation (tenths of mm)
    /// </summary>
    PRCP,
    /// <summary>
    /// Snowfall (mm)
    /// </summary>
    SNOW,
    /// <summary>
    /// Snow depth (mm)
    /// </summary>
    SNWD,
    /// <summary>
    /// Maximum temperature (tenths of degrees C)
    /// </summary>
    TMAX,
    /// <summary>
    /// Minimum temperature (tenths of degrees C)
    /// </summary>
    TMIN,
    /// <summary>
    /// Average temperature (tenths of degrees C) [Note that TAVG from source 'S' corresponds
    /// to an average for the period ending at 2400 UTC rather than local midnight]
    /// </summary>
    TAVG
}

public enum MeasurementFlag
{
    /// <summary>
    /// Blank = no measurement information applicable
    /// </summary>
    Blank,
    /// <summary>
    /// precipitation total formed from two 12-hour totals
    /// </summary>
    B,
    /// <summary>
    /// precipitation total formed from four six-hour totals
    /// </summary>
    D,
    /// <summary>
    /// represents highest or lowest hourly temperature (TMAX or TMIN) or the average of hourly values (TAVG)
    /// </summary>
    H,
    /// <summary>
    /// converted from knots 
    /// </summary>
    K,
    /// <summary>
    /// temperature appears to be lagged with respect to reported our of observation 
    /// </summary>
    L,
    /// <summary>
    /// converted from oktas 
    /// </summary>
    O,
    /// <summary>
    /// identified as "missing presumed zero" in DSI 3200 and 3206
    /// </summary>
    P,
    /// <summary>
    /// trace of precipitation, snowfall, or snow depth
    /// </summary>
    T,
    /// <summary>
    /// converted from 16-point WBAN code (for wind direction)
    /// </summary>
    W,
}

public enum QualityFlag
{
    /// <summary>
    /// did not fail any quality assurance check
    /// </summary>
    Blank,
    /// <summary>
    /// failed duplicate check
    /// </summary>
    D,
    /// <summary>
    /// failed gap check
    /// </summary>
    G,
    /// <summary>
    /// failed internal consistency check
    /// </summary>
    I,
    /// <summary>
    /// failed streak/frequent-value check
    /// </summary>
    K,
    /// <summary>
    /// failed check on length of multiday period
    /// </summary>
    L,
    /// <summary>
    /// failed megaconsistency check
    /// </summary>
    M,
    /// <summary>
    /// failed naught check
    /// </summary>
    N,
    /// <summary>
    /// failed climatological outlier check
    /// </summary>
    O,
    /// <summary>
    /// failed lagged range check
    /// </summary>
    R,
    /// <summary>
    /// failed spatial consistency check
    /// </summary>
    S,
    /// <summary>
    /// failed temporal consistency check
    /// </summary>
    T,
    /// <summary>
    /// temperature too warm for snow
    /// </summary>
    W,
    /// <summary>
    /// failed bounds 
    /// </summary>
    X,
    /// <summary>
    /// flagged as a result of an official Datzilla investigation
    /// </summary>
    Z,
}

public enum SourceFlag
{
    ///<summary>
    ///No source (i.e., data value missing)
    ///</summary>
    Blank,
    ///<summary>
    ///U.S. Cooperative Summary of the Day (NCDC DSI-3200)
    ///</summary>       
    _0,
    ///<summary>
    ///CDMP Cooperative Summary of the Day (NCDC DSI-3206)
    ///</summary>       
    _6,
    ///<summary>
    ///U.S. Cooperative Summary of the Day -- Transmitted via WxCoder3 (NCDC DSI-3207)
    ///</summary>       
    _7,
    ///<summary>
    ///U.S.Automated Surface Observing System (ASOS) real-time data (since January 1, 2006)
    ///</summary>       
    A,
    ///<summary>
    ///Australian data from the Australian Bureau of Meteorology
    ///</summary>   
    a,
    ///<summary>
    ///U.S.ASOS data for October 2000-December 2005 (NCDC DSI-3211)
    ///</summary>       
    B,
    ///<summary>
    ///Belarus update
    ///</summary>   
    b,
    ///<summary>
    ///Environment Canada
    ///</summary>   
    C,

    ///<summary>
    ///Short time delay US National Weather Service CF6 daily summaries provided by the High Plains Regional Climate Center
    ///</summary>   
    D,

    ///<summary>
    ///European Climate Assessment and Dataset (Klein Tank et al., 2002)	   
    ///</summary>   
    E,
    ///<summary>
    ///U.S.Fort data
    ///</summary>       
    F,
    ///<summary>
    ///Official Global Climate Observing System (GCOS) or other government-supplied data
    ///</summary>       
    G,
    ///<summary>
    ///High Plains Regional Climate Center real-time data
    ///</summary>       
    H,
    ///<summary>
    ///International collection (non U.S. data received through personal contacts)
    ///</summary>       
    I,
    ///<summary>
    ///U.S.Cooperative Summary of the Day data digitized from paper observer forms (from 2011 to present)
    ///</summary>       
    K,
    ///<summary>
    ///Monthly METAR Extract (additional ASOS data)
    ///</summary>       
    M,
    ///<summary>
    ///Data from the Mexican National Water Commission (Comision National del Agua -- CONAGUA)
    ///</summary>   
    m,
    ///<summary>
    ///Community Collaborative Rain, Hail, and Snow (CoCoRaHS)
    ///</summary>   
    N,
    ///<summary>
    ///Data from several African countries that had been "quarantined", that is, withheld from public release until permission was granted from the respective meteorological services
    ///</summary>   
    Q,
    ///<summary>
    ///NCEI Reference Network Database(Climate Reference Network and Regional Climate Reference Network)
    ///</summary>       
    R,
    ///<summary>
    ///All-Russian Research Institute of Hydrometeorological Information-World Data Center
    ///</summary>   
    r,
    ///<summary>
    ///Global Summary of the Day(NCDC DSI-9618) NOTE: "S" values are derived from hourly synoptic reports exchanged on the Global Telecommunications System(GTS). Daily values derived in this fashion may differ significantly  from "true" daily data, particularly for precipitation (i.e., use with caution).
    ///</summary>       
    S,
    ///<summary>
    ///China Meteorological Administration/National Meteorological Information Center/Climatic Data Center (http://cdc.cma.gov.cn
    ///</summary>   
    s,
    ///<summary>
    ///SNOwpack TELemtry (SNOTEL) data obtained from the U.S. Department of Agriculture's Natural Resources Conservation Service
    ///</summary>   
    T,
    ///<summary>
    ///Remote Automatic Weather Station (RAWS) data obtained from the Western Regional Climate Center
    ///</summary>   
    U,
    ///<summary>
    ///Ukraine update
    ///</summary>   
    u,
    ///<summary>
    ///WBAN/ASOS Summary of the Day from NCDC's Integrated Surface Data (ISD).  
    ///</summary>   
    W,
    ///<summary>
    ///U.S. First-Order Summary of the Day (NCDC DSI-3210)
    ///</summary>       
    X,
    ///<summary>
    ///Datzilla official additions or replacements
    ///</summary>   
    Z,
    ///<summary>
    ///Uzbekistan update
    ///</summary>   
    z,
}