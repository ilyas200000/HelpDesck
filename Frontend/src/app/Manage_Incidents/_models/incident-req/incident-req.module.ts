

export interface Incident {
  INCD_DESC: string;
  INCD_PRIO_ID: number;
  INCD_TYPE_ID: number;
  agn_code: string;
  INCD_STAT_ID: number;
  INCD_UTIL_ID: number;

}

export interface Priorite {
 
  INCD_PRIO_ID: number;
  PRIO_DESC: string;
}
export interface Type {
 
  INCD_TYPE_ID: number;
  TYPE_DESC: string;
}
export interface Statut {
 
  INCD_STAT_ID: number;
  STAT_DESC: string;
}

