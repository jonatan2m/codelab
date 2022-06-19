import MoviesDAO from "../src/dao/moviesDAO"

describe("Paging", () => {
  beforeAll(async () => {
    await MoviesDAO.injectDB(global.mflixClient)
  })

  test("Supports paging by cast", async () => {
    const filters = { cast: ["Tom Hanks", "Natalie Portman"] }
    /**
     * Testing first page
     */
    const { moviesList: firstPage, totalNumMovies } = await MoviesDAO.getMovies(
      {
        filters,
      },
    )

    // check the total number of movies, including both pages
    expect(totalNumMovies).toEqual(60)

    // check the number of movies on the first page
    expect(firstPage.length).toEqual(20)

    // check some of the movies on the second page
    const firstMovie = firstPage[0]
    const twentiethMovie = firstPage.slice(-1).pop()
    expect(firstMovie.title).toEqual(
      "Star Wars: Episode III - Revenge of the Sith",
    )
    expect(twentiethMovie.title).toEqual("Sleepless in Seattle")

    /**
     * Testing second page
     */
    const { moviesList: secondPage } = await MoviesDAO.getMovies({
      filters,
      page: 1,
    })

    // check the number of movies on the second page
    expect(secondPage.length).toEqual(20)
    // check some of the movies on the second page
    const twentyFirstMovie = secondPage[0]
    const twentyNinthMovie = secondPage.slice(-1).pop()
    expect(twentyFirstMovie.title).toEqual("Lèon: The Professional")
    expect(twentyNinthMovie.title).toEqual("Your Highness")

    /**
     * Testing third page
     */
    const { moviesList: thirdPage } = await MoviesDAO.getMovies({
      filters,
      page: 2,
    })

    // check the number of movies on the second page
    expect(thirdPage.length).toEqual(20)
    // check some of the movies on the second page
    const thirtyFirstMovie = thirdPage[0]
    const lastMovie = thirdPage.slice(-1).pop()
    expect(thirtyFirstMovie.title).toEqual("Goya's Ghosts")
    expect(lastMovie.title).toEqual("The Da Vinci Code")
  })

  test("Supports paging by genre", async () => {
    const filters = { genre: ["Comedy", "Drama"] }

    /**
     * Testing first page
     */
    const { moviesList: firstPage, totalNumMovies } = await MoviesDAO.getMovies(
      {
        filters,
      },
    )

    // check the total number of movies, including both pages
    expect(totalNumMovies).toEqual(17903)

    // check the number of movies on the first page
    expect(firstPage.length).toEqual(20)

    // check some of the movies on the second page
    const firstMovie = firstPage[0]
    const twentiethMovie = firstPage.slice(-1).pop()
    expect(firstMovie.title).toEqual("Titanic")
    expect(twentiethMovie.title).toEqual("Dègkeselyè")

    /**
     * Testing second page
     */
    const { moviesList: secondPage } = await MoviesDAO.getMovies({
      filters,
      page: 1,
    })

    // check the number of movies on the second page
    expect(secondPage.length).toEqual(20)
    // check some of the movies on the second page
    const twentyFirstMovie = secondPage[0]
    const fortiethMovie = secondPage.slice(-1).pop()
    expect(twentyFirstMovie.title).toEqual("8 Mile")
    expect(fortiethMovie.title).toEqual("Forrest Gump")
  })

  test("Supports paging by text", async () => {
    const filters = { text: "countdown" }

    /**
     * Testing first page
     */
    const { moviesList: firstPage, totalNumMovies } = await MoviesDAO.getMovies(
      {
        filters,
      },
    )

    // check the total number of movies, including both pages
    expect(totalNumMovies).toEqual(12)

    // check the number of movies on the first page
    expect(firstPage.length).toEqual(12)

    // check some of the movies on the second page
    const firstMovie = firstPage[0]
    const twentiethMovie = firstPage.slice(-1).pop()
    expect(firstMovie.title).toEqual("Countdown")
    expect(twentiethMovie.title).toEqual("The Front Line")

    /**
     * Testing second page
     */
    const { moviesList: secondPage } = await MoviesDAO.getMovies({
      filters,
      page: 1,
    })

    // check the number of movies on the second page
    expect(secondPage.length).toEqual(0)
  })
})
