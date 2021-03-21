export interface ICatalog {
    id: string;
    isSuccess: boolean;
    isFailure: boolean;
    data: {
      id: string;
      clusterID: number;
      title: string;
      genreID: number;
      genereTitle: string;
      releaseDate: Date;
      companyID: string;
      companyName: string;
      price: number;
      rate: number
    }
  };
