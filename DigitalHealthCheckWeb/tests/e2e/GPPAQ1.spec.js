describe("GPPAQ1 page", () => {
    const pageToTest = '<digitalHealthCheckURL>/DigitalHealthCheck2_TEST/GPPAQ1';

    beforeEach(async () => {
        await jestPlaywright.resetPage();
        await page.goto(pageToTest);
    })

    it("should show a heading", async () => {
        // after by using expect-playwright
        await expect(page).toMatchText("h1", "Physical activity page 2 of 4");
    })

    it("should not go onto next page if no option selected", async () => {
        // Click text=Continue
        await page.click('text=Continue');
        expect(page.url().startsWith('<digitalHealthCheckURL>/DigitalHealthCheck2_TEST/GPPAQ1')).toBeTruthy();
    })

    it("should not go onto next page if only one question answered", async () => {
          // Click text=None
        await page.click('text=None');
        const checked = await page.isChecked('#physical-activity', 'none');
        expect(checked).toBeTruthy();
        await page.click('text=Continue');
        expect(page.url().startsWith('<digitalHealthCheckURL>/DigitalHealthCheck2_TEST/GPPAQ1')).toBeTruthy();
        await expect(page).toMatchText("h1", "Physical activity page 2 of 4");
 
    }) 

    it(" goes to GPPAQ2 if both questions answered", async () => {
        // Click text=Some but less than 1 hour
        await page.click('text=Some but less than 1 hour');
        // Click text=Between 1 and 3 hours
        await page.click(':nth-match(:text("Some but less than 1 hour"), 2)');
        const checked = page.isChecked('#housework-2','lessthanonehour');
        expect(checked).toBeTruthy();
        await page.click('text=Continue');
        expect(page.url().startsWith('<digitalHealthCheckURL>/DigitalHealthCheck2_TEST/GPPAQ2')).toBeTruthy();
        await expect(page).toMatchText("h1", "Physical activity page 3 of 4");

  })  

  it("should have a back link", async () => {
    // Click text=Back
    await page.click('text=Back');
    expect(page.url().startsWith('<digitalHealthCheckURL>/DigitalHealthCheck2_TEST/PhysicalActivity')).toBeTruthy();
  })

})