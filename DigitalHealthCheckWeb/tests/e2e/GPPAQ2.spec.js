describe("GPPAQ2 page", () => {
    const pageToTest = '<digitalHealthCheckURL>/DigitalHealthCheck2_TEST/GPPAQ2';

    beforeEach(async () => {
        await jestPlaywright.resetPage();
        await page.goto(pageToTest);
    })

    it("should show a heading", async () => {
        // after by using expect-playwright
        await expect(page).toMatchText("h1", "Physical activity page 3 of 4");
    })

    it("should not go onto next page if no option selected", async () => {
        // Click text=Continue
        await page.click('text=Continue');
        expect(page.url().startsWith('<digitalHealthCheckURL>/DigitalHealthCheck2_TEST/GPPAQ2')).toBeTruthy();
    })

    it("should not go onto next page if only one question answered", async () => {
          // Click text=Some but less than 1 hour
        await page.click('text=Some but less than 1 hour');
        const checked = await page.isChecked('#gardening-2', 'lessthanonehour');
        expect(checked).toBeTruthy();
        await page.click('text=Continue');
        expect(page.url().startsWith('<digitalHealthCheckURL>/DigitalHealthCheck2_TEST/GPPAQ2')).toBeTruthy();
        await expect(page).toMatchText("h1", "Physical activity page 3 of 4");
 
    }) 

    it(" goes to GPPAQ3 if both questions answered", async () => {
        // Click text=Between 1 and 3 hours
        await page.click('text=Between 1 and 3 hours');
        // Click text=Between 1 and 3 hours
        await page.click('#housework-4','threehoursormore');
        const checked = page.isChecked('#housework-4','threehoursormore');
        expect(checked).toBeTruthy();
        await page.click('text=Continue');
        expect(page.url().startsWith('<digitalHealthCheckURL>/DigitalHealthCheck2_TEST/GPPAQ3')).toBeTruthy();
        await expect(page).toMatchText("h1", "Physical activity page 4 of 4");

  })  

  it("should have a back link", async () => {
    // Click text=Back
    await page.click('text=Back');
    expect(page.url().startsWith('<digitalHealthCheckURL>/DigitalHealthCheck2_TEST/GPPAQ1')).toBeTruthy();
  })

})