export interface IGenreResult {
  id: string;
  isSuccess: boolean;
  isFailure: boolean;
  data: {
    id: number;
    title: string;
    selected: boolean
  }
};
